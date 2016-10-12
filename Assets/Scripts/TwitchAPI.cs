using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using SimpleJSON;

public class TwitchAPI : MonoBehaviour {

	private static string CLIENT_ID = "9r82dk02lbj7cdk5ln8fkxzqnzvt4ic";
	private static string CLIENT_SECRET = "j8nak4k4mc82n7hyohv8dmpchvvr3dd";
	private static string ACCESS_TOKEN = "no2q4269jkyhmrhnlnd6ki9md5q602";

	private static TwitchAPI instance;

	void Awake()
	{
		instance = this;
	}

	public static void Uptime(Action<string> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://api.twitch.tv/kraken/streams/xliii",
			success =>
			{
				var root = JSON.Parse(success);
				var stream = root["stream"];
				if (stream.Value == "null")
				{
					Debug.Log("Stream offline");
					callback("Stream offline");
				} else
				{
					var createdAt = stream["created_at"];
					Debug.Log("Stream launched at " + createdAt);
					callback("Stream started at " + createdAt);
				}
			},
			error =>
			{
				Debug.LogError("Error while processing !uptime: " + error);
				callback("ERROR! Couldn't retrieve uptime :(");
			}
		));
	}

	public static void SetGame(string game, Action<string> callback)
	{
		string body = "{\"channel\":{\"game\":\"" + game + "\"}}";
		instance.StartCoroutine(instance.PutRequest ("https://api.twitch.tv/kraken/channels/xliii", body,
			success =>
			{
				callback("Game updated");
			},
			error =>
			{
				callback("ERROR! Couldn't update game :(");
			}
			));
	}

	public static void SetTitle(string title, Action<string> callback)
	{
		string body = "{\"channel\":{\"status\":\"" + title + "\"}}";
		instance.StartCoroutine(instance.PutRequest("https://api.twitch.tv/kraken/channels/xliii", body,
			success =>
			{
				callback("Title updated");
			},
			error =>
			{
				callback("ERROR! Couldn't update title :(");
			}));
	}

	private IEnumerator GetRequest(string url, Action<string> onSuccess, Action<string> onError)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			www.SetRequestHeader("Accept", "application/vnd.twitchtv.v3+json");
			www.SetRequestHeader("Client-ID", CLIENT_ID);
			//www.SetRequestHeader("Authorization", "OAuth " + ACCESS_TOKEN);
			//www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isError)
			{
				onError(www.error);
			}
			else
			{
				onSuccess(www.downloadHandler.text);
			}
		}
	}

	private IEnumerator PutRequest(string url, string body, Action<string> onSuccess, Action<string> onError)
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
				onError(www.error);
			}
			else
			{
				onSuccess(www.downloadHandler.text);
			}
		}
	}
	
}
