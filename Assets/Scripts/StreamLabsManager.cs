using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class StreamLabsManager : MonoBehaviour {

	private string ACCESS_TOKEN;
	private string REFRESH_TOKEN;
	private int EXPIRES_IN;

	private const string PREFS_REFRESH = "stream-labs-refresh-token";

	public static int LIMIT = 5;
	public static string CURRENCY = "EUR";

	private const string SCOPE = "donations.read";

	private const string TOKEN_URL = "https://streamlabs.com/api/v1.0/token";
	private const string USER_URL = "https://www.twitchalerts.com/api/v1.0/user?access_token={0}";
	private const string INITIAL_DONATIONS_URL = "https://www.twitchalerts.com/api/v1.0/donations?access_token={0}&currency={1}&limit=1";
	private const string DONATIONS_URL = "https://www.twitchalerts.com/api/v1.0/donations?access_token={0}&currency={1}&limit={2}&after={3}";
	private const string AUTHORIZE_URL = "https://streamlabs.com/api/v1.0/authorize?response_type=code&client_id={0}&redirect_uri={1}&scope={2}";

	private const int UPDATE_RATE = 10000; //60 seconds

	System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

	private int latestDonationId = 0;
	private string CLIENT_ID = "iFQ71QQzZIfl4s9TTx2pFW27YBOSOARZgve6g7Sc";
	private string CLIENT_SECRET = "4lGIGViZbvdx5z1or4eLAUkvworjJliJJjzP88qx";
	private string REDIRECT_URI = "http://127.0.0.1:3000/streamlabs";

	//TODO: StreamLabs ids are not actually strictly increasing, handle that mb?
	void Start ()
	{
		REFRESH_TOKEN = PlayerPrefs.GetString(PREFS_REFRESH, "");
		if (!string.IsNullOrEmpty(REFRESH_TOKEN))
		{
			StartCoroutine(RefreshToken(true));
			return;
		}

		Messenger.AddListener<string>(Events.STREAM_LABS_CODE, code =>
		{
			StartCoroutine(FetchToken(code));
		});

		//TODO: Open browser for StreamLabs authorization
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
			yield return RefreshToken(false);
		}

		Debug.Log("Polling for donations");
		yield return FetchDonations();
	}

	private void TokenRetrieved(string accessToken, string refreshToken, bool initial)
	{
		Debug.Log(initial ? "Authorized with StreamLabs" : "StreamLabs token refreshed");
		ACCESS_TOKEN = accessToken;
		EXPIRES_IN = TimeUtils.UnixTimestamp() + 3600; //1 hour

		REFRESH_TOKEN = refreshToken;
		PlayerPrefs.SetString(PREFS_REFRESH, REFRESH_TOKEN);
		PlayerPrefs.Save();

		if (!initial) return;

		//Load latest donation ID
		StartCoroutine(FetchDonations(true));
		stopWatch.Start();
	}

	private void DonationsRetrieved(List<DonationAlertData> donations, bool ignore)
	{
		foreach (var donation in donations)
		{
			latestDonationId = Mathf.Max(donation.id, latestDonationId);
			Debug.Log("Latest donation by " + donation.username);
			if (!ignore)
			{
				Debug.Log("Donation: " + donation.username + " -> " + donation.amountFormatted);
				Messenger.Broadcast(TwitchAlertsType.most_recent_donator.ToString(), donation);
			}
		}
	}

	IEnumerator Authorize()
	{
		Debug.Log("Authorize");
		var url = string.Format(AUTHORIZE_URL, CLIENT_ID, REDIRECT_URI, SCOPE);
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isNetworkError)
			{
				Debug.LogError(url + " GET -> " + www.error);
			}
			else
			{
				Debug.Log(url + " GET -> " + www.downloadHandler.text);
			}
		}
	}

	IEnumerator RefreshToken(bool initial)
	{
		Debug.Log("Refresh token");
		var url = TOKEN_URL;
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "refresh_token");
		form.AddField("client_id", CLIENT_ID);
		form.AddField("client_secret", CLIENT_SECRET); //TODO: Hide
		form.AddField("redirect_uri", REDIRECT_URI);
		form.AddField("refresh_token", REFRESH_TOKEN);
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isNetworkError)
			{
				Debug.LogError(url + " POST -> " + www.error);
			}
			else
			{
				Debug.Log(url + " POST -> " + www.downloadHandler.text);
				var root = JSON.Parse(www.downloadHandler.text);
				TokenRetrieved(root["access_token"], root["refresh_token"], initial);
			}
		}
	}

	IEnumerator FetchToken(string code)
	{
		var url = TOKEN_URL;
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "authorization_code");
		form.AddField("client_id", CLIENT_ID);
		form.AddField("client_secret", CLIENT_SECRET); //TODO: Hide
		form.AddField("redirect_uri", REDIRECT_URI);
		form.AddField("code", code);
		using (UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			yield return www.Send();

			if (www.isNetworkError)
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

	IEnumerator FetchDonations(bool initial = false)
	{
		string url;
		if (initial)
		{
			url = string.Format(INITIAL_DONATIONS_URL, ACCESS_TOKEN, CURRENCY);
		}
		else
		{
			url = string.Format(DONATIONS_URL, ACCESS_TOKEN, CURRENCY, LIMIT, latestDonationId);
		}
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isNetworkError)
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
				DonationsRetrieved(donations, initial);
			}
		}
	}

	IEnumerator FetchUser()
	{
		var url = string.Format(USER_URL, ACCESS_TOKEN);
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			yield return www.Send();

			if (www.isNetworkError)
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
