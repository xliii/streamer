using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class StreamLabsManager : MonoBehaviour {

	private string ACCESS_TOKEN;
	private string REFRESH_TOKEN;

	private const string TOKEN_URL = "https://www.twitchalerts.com/api/v1.0/token";
	private const string USER_URL = "https://www.twitchalerts.com/api/v1.0/user?access_token={0}";
	private const string DONATIONS_URL = "https://www.twitchalerts.com/api/v1.0/donations?access_token={0}&limit={1}&currency={2}";

	private int latestTimestamp = 0;

	void Awake()
	{
		//latestTimestamp = TimeUtils.UnixTimestamp();
	}

	// Use this for initialization
	void Start () {
		Messenger.AddListener<string>(Events.STREAM_LABS_CODE, code =>
		{
			Debug.Log("Received code: " + code);
			StartCoroutine(FetchToken(code));
		});
	}

	private void TokenRetrieved(string accessToken, string refreshToken)
	{
		ACCESS_TOKEN = accessToken;
		REFRESH_TOKEN = refreshToken;
		Debug.Log("Access: " + accessToken);
		Debug.Log("Refresh: " + refreshToken);
		StartCoroutine(FetchDonations("EUR", 5));
	}

	private void DonationsRetrieved(List<DonationAlertData> donations)
	{
		int maxTimestamp = 0;
		foreach (var donation in donations)
		{
			if (donation.timestamp < latestTimestamp) continue;

			maxTimestamp = Mathf.Max(maxTimestamp, donation.timestamp);
			Debug.Log("Donation: " + donation.username + " -> " + donation.amountFormatted);
			Messenger.Broadcast(TwitchAlertsType.most_recent_donator.ToString(), donation);
		}

		latestTimestamp = Mathf.Max(latestTimestamp, maxTimestamp);
	}

	IEnumerator FetchToken(string code)
	{
		WWWForm form = new WWWForm();
		form.AddField("grant_type", "authorization_code");
		form.AddField("client_id", "iFQ71QQzZIfl4s9TTx2pFW27YBOSOARZgve6g7Sc");
		form.AddField("client_secret", "4lGIGViZbvdx5z1or4eLAUkvworjJliJJjzP88qx"); //TODO: Hide
		form.AddField("redirect_uri", "http://127.0.0.1:3000/streamlabs");
		form.AddField("code", code);
		using (UnityWebRequest www = UnityWebRequest.Post(TOKEN_URL, form))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError("Error: " + www.error);
			} else
			{
				var root = JSON.Parse(www.downloadHandler.text);
				TokenRetrieved(root["access_token"], root["refresh_token"]);
			}
		}
	}

	IEnumerator FetchDonations(string currency, int limit)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(string.Format(DONATIONS_URL, ACCESS_TOKEN, limit, currency)))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError("Error: " + www.error);
			}
			else
			{
				var donations = new List<DonationAlertData>();
				var root = JSON.Parse(www.downloadHandler.text);
				foreach (JSONNode donation in root["data"].AsArray)
				{
					donations.Add(DonationAlertData.Create(donation["name"], donation["amount"].AsFloat, donation["message"], donation["created_at"].AsInt) as DonationAlertData);
				}
				DonationsRetrieved(donations);
			}
		}
	}

	IEnumerator FetchUser()
	{
		using (UnityWebRequest www = UnityWebRequest.Get(string.Format(USER_URL, ACCESS_TOKEN)))
		{
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError("Error: " + www.error);
			}
			else
			{
				Debug.Log("Success: " + www.downloadHandler.text);
			}
		}
	}
}
