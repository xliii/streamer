using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;

public class TwitchAPI : MonoBehaviour {

	private static TwitchAPI instance;

	void Awake()
	{
		instance = this;
	}

	public static void Uptime(Action<bool, string> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://api.twitch.tv/kraken/streams/xliii",
			success =>
			{
				var root = JSON.Parse(success);
				var stream = root["stream"];
				if (stream.Value == "null")
				{
					callback(true, null);
				} else
				{
					var createdAt = stream["created_at"].Value;
					callback(true, createdAt);
				}
			},
			error =>
			{
				callback(false, error);
			}
		));
	}

	public static void SetGame(string game, Action<bool> callback)
	{
		string body = "{\"channel\":{\"game\":\"" + game + "\"}}";
		instance.StartCoroutine(instance.PutRequest ("https://api.twitch.tv/kraken/channels/xliii", body,
			success =>
			{
				callback(true);
			},
			error =>
			{
				callback(false);
			}
			));
	}

	public static void SetTitle(string title, Action<bool> callback)
	{
		string body = "{\"channel\":{\"status\":\"" + title + "\"}}";
		instance.StartCoroutine(instance.PutRequest("https://api.twitch.tv/kraken/channels/xliii", body,
			success =>
			{
				callback(true);
			},
			error =>
			{
				callback(false);
			}));
	}

	public static void GetViewers(Action<Dictionary<string, UserRole>> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://tmi.twitch.tv/group/user/xliii/chatters",
			success =>
			{
				Dictionary<string, UserRole> dict = new Dictionary<string, UserRole>();
				var root = JSON.Parse(success);
				var chatters = root["chatters"];
				foreach (JSONNode mod in chatters["moderators"].AsArray)
				{
					dict[mod.ToString().Replace("\"", "")] = UserRole.Mod;
				}
				foreach (JSONNode staff in chatters["staff"].AsArray)
				{
					dict[staff.ToString().Replace("\"", "")] = UserRole.Staff;
				}
				foreach (JSONNode admin in chatters["admins"].AsArray)
				{
					dict[admin.ToString().Replace("\"", "")] = UserRole.Admin;
				}
				foreach (JSONNode globalMod in chatters["global_mods"].AsArray)
				{
					dict[globalMod.ToString().Replace("\"", "")] = UserRole.GlobalMod;
				}
				foreach (JSONNode viewer in chatters["viewers"].AsArray)
				{
					dict[viewer.ToString().Replace("\"", "")] = UserRole.Viewer;
				}

				callback(dict);
			},
			error =>
			{
				Debug.LogError(error);
			}));
	}

	private IEnumerator GetRequest(string url, Action<string> onSuccess, Action<string> onError)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			www.SetRequestHeader("Accept", "application/vnd.twitchtv.v3+json");
			www.SetRequestHeader("Client-ID", Config.Get(Config.API_CLIENT_ID));
			//www.SetRequestHeader("Authorization", "OAuth " + Config.Get(Config.API_ACCESS_TOKEN));
			//www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " GET -> " + www.error);
				onError(www.error);
			}
			else
			{
				Debug.Log(url + " GET -> " + www.downloadHandler.text);
				onSuccess(www.downloadHandler.text);
			}
		}
	}

	private IEnumerator PutRequest(string url, string body, Action<string> onSuccess, Action<string> onError)
	{
		using (UnityWebRequest www = UnityWebRequest.Put(url, body))
		{
			www.SetRequestHeader("Accept", "application/vnd.twitchtv.v3+json");
			www.SetRequestHeader("Client-ID", Config.Get(Config.API_CLIENT_ID));
			www.SetRequestHeader("Authorization", "OAuth " + Config.Get(Config.API_ACCESS_TOKEN));
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isError)
			{
				Debug.LogError(url + " PUT -> " + www.error);
				onError(www.error);
			}
			else
			{
				Debug.Log(url + " PUT -> " + www.downloadHandler.text);
				onSuccess(www.downloadHandler.text);
			}
		}
	}
	
}
