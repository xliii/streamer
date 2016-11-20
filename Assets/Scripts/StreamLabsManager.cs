using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class StreamLabsManager : MonoBehaviour {

	private string ACCESS_TOKEN;
	private string REFRESH_TOKEN;
	private int EXPIRES_IN;

	public static int LIMIT = 5;
	public static string CURRENCY = "EUR";

	private const string TOKEN_URL = "https://www.twitchalerts.com/api/v1.0/token";
	private const string USER_URL = "https://www.twitchalerts.com/api/v1.0/user?access_token={0}";
	private const string DONATIONS_URL = "https://www.twitchalerts.com/api/v1.0/donations?access_token={0}&currency={1}&limit={2}&after={3}";

	private const int UPDATE_RATE = 10000; //60 seconds

	System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

	private int latestDonationId = 0;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<string>(Events.STREAM_LABS_CODE, code =>
		{
			StartCoroutine(FetchToken(code));
		});
	}

	void Update()
	{
		if (stopWatch.ElapsedMilliseconds > UPDATE_RATE)
		{
			StartCoroutine(CheckTokenAndFetchDonations());
			stopWatch.Reset();
			stopWatch.Start();
		}
	}

	IEnumerator CheckTokenAndFetchDonations()
	{
		if (EXPIRES_IN <= TimeUtils.UnixTimestamp())
		{
			yield return RefreshToken();
		}

		Debug.Log("Polling for donations");
		yield return FetchDonations(5);
	}

	private void TokenRetrieved(string accessToken, string refreshToken, bool initial)
	{
		Debug.Log(initial ? "Authorized with StreamLabs" : "StreamLabs token refreshed");
		ACCESS_TOKEN = accessToken;
		REFRESH_TOKEN = refreshToken;
		EXPIRES_IN = TimeUtils.UnixTimestamp() + 3600; //1 hour

		if (!initial) return;

		//Load latest donation ID
		StartCoroutine(FetchDonations(1, true));
		stopWatch.Start();
	}

	private void DonationsRetrieved(List<DonationAlertData> donations, bool ignore)
	{
		foreach (var donation in donations)
		{
			latestDonationId = Mathf.Max(donation.id, latestDonationId);
			if (!ignore)
			{
				Debug.Log("Donation: " + donation.username + " -> " + donation.amountFormatted);
				Messenger.Broadcast(TwitchAlertsType.most_recent_donator.ToString(), donation);
			}
		}
	}

	IEnumerator RefreshToken()
	{
		var url = TOKEN_URL;
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "refresh_token");
		form.AddField("client_id", "iFQ71QQzZIfl4s9TTx2pFW27YBOSOARZgve6g7Sc");
		form.AddField("client_secret", "4lGIGViZbvdx5z1or4eLAUkvworjJliJJjzP88qx"); //TODO: Hide
		form.AddField("redirect_uri", "http://127.0.0.1:3000/streamlabs");
		form.AddField("refresh_token", REFRESH_TOKEN);
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " POST -> " + www.error);
			}
			else
			{
				Debug.Log(url + " POST -> " + www.downloadHandler.text);
				var root = JSON.Parse(www.downloadHandler.text);
				TokenRetrieved(root["access_token"], root["refresh_token"], false);
			}
		}
	}

	IEnumerator FetchToken(string code)
	{
		var url = TOKEN_URL;
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "authorization_code");
		form.AddField("client_id", "iFQ71QQzZIfl4s9TTx2pFW27YBOSOARZgve6g7Sc");
		form.AddField("client_secret", "4lGIGViZbvdx5z1or4eLAUkvworjJliJJjzP88qx"); //TODO: Hide
		form.AddField("redirect_uri", "http://127.0.0.1:3000/streamlabs");
		form.AddField("code", code);
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " POST -> " + www.error);
			} else
			{
				Debug.Log(url + " POST -> " + www.downloadHandler.text);
				var root = JSON.Parse(www.downloadHandler.text);
				TokenRetrieved(root["access_token"], root["refresh_token"], true);
			}
		}
	}

	IEnumerator FetchDonations(int limit, bool ignore = false)
	{
		var url = string.Format(DONATIONS_URL, ACCESS_TOKEN, CURRENCY, limit, latestDonationId);
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " GET -> " + www.error);
			}
			else
			{
				Debug.Log(url + " GET -> " + www.downloadHandler.text);
				var donations = new List<DonationAlertData>();
				var root = JSON.Parse(www.downloadHandler.text);
				foreach (JSONNode donation in root["data"].AsArray)
				{
					donations.Add(DonationAlertData.Create(donation["name"], donation["amount"].AsFloat, donation["message"], donation["donation_id"].AsInt) as DonationAlertData);
				}
				DonationsRetrieved(donations, ignore);
			}
		}
	}

	IEnumerator FetchUser()
	{
		var url = string.Format(USER_URL, ACCESS_TOKEN);
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " GET -> " + www.error);
			}
			else
			{
				Debug.Log(url + " GET -> " + www.downloadHandler.text);
			}
		}
	}
}
