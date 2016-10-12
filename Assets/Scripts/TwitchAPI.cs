using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TwitchAPI : MonoBehaviour {

	private static string CLIENT_ID = "9r82dk02lbj7cdk5ln8fkxzqnzvt4ic";
	private static string CLIENT_SECRET = "j8nak4k4mc82n7hyohv8dmpchvvr3dd";
	private static string ACCESS_TOKEN = "no2q4269jkyhmrhnlnd6ki9md5q602";

	private static TwitchAPI instance;

	void Awake()
	{
		instance = this;
	}

	public static void SetGame(string game)
	{
		string body = "{\"channel\":{\"game\":\"" + game + "\"}}";
		instance.StartCoroutine(instance.PutRequest ("https://api.twitch.tv/kraken/channels/xliii", body));
	}

	public static void SetTitle(string title)
	{
		string body = "{\"channel\":{\"status\":\"" + title + "\"}}";
		instance.StartCoroutine(instance.PutRequest("https://api.twitch.tv/kraken/channels/xliii", body));
	}

	private IEnumerator PutRequest(string url, string body)
	{
		using (UnityWebRequest www = UnityWebRequest.Put(url, body))
		{
			www.SetRequestHeader("Accept", "application/vnd.twitchtv.v3+json");
			www.SetRequestHeader("Client-ID", CLIENT_ID);
			www.SetRequestHeader("Authorization", "OAuth " + ACCESS_TOKEN);
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(www.error);
			}
			else
			{
				Debug.Log(www.downloadHandler.text);
			}
		}
	}
	
}
