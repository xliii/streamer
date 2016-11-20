using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;

public class StreamLabsManager : MonoBehaviour {

	private string ACCESS_TOKEN;
	private string REFRESH_TOKEN;

	public static int LIMIT = 5;
	public static string CURRENCY = "EUR";

	private const string TOKEN_URL = "https://www.twitchalerts.com/api/v1.0/token";
	private const string USER_URL = "https://www.twitchalerts.com/api/v1.0/user?access_token={0}";
	private const string DONATIONS_URL = "https://www.twitchalerts.com/api/v1.0/donations?access_token={0}&currency={1}&limit={2}&after={3}";

	private int latestDonationId = 0;

	// Use this for initialization
	void Start () {
		Messenger.AddListener<string>(Events.STREAM_LABS_CODE, code =>
		{
			StartCoroutine(FetchToken(code));
		});
	}

	private void TokenRetrieved(string accessToken, string refreshToken)
	{
		Debug.Log("Authorized with StreamLabs");
		ACCESS_TOKEN = accessToken;
		REFRESH_TOKEN = refreshToken;
		//Load latest donation ID
		StartCoroutine(FetchDonations(1, true));
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

	IEnumerator FetchDonations(int limit, bool ignore = false)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(string.Format(DONATIONS_URL, ACCESS_TOKEN, CURRENCY, limit, latestDonationId)))
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
					donations.Add(DonationAlertData.Create(donation["name"], donation["amount"].AsFloat, donation["message"], donation["donation_id"].AsInt) as DonationAlertData);
				}
				DonationsRetrieved(donations, ignore);
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
