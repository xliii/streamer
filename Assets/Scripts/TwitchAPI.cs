﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;
using SimpleJSON;
using System.Collections.Generic;

public class TwitchAPI : MonoBehaviour {

	private static TwitchAPI instance;

	private void Awake()
	{
		instance = this;
	}

	public static void Geolocate(string place, Action<string, float, float> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://maps.googleapis.com/maps/api/geocode/json?&address=" + place,
			success =>
			{
				var root = JSON.Parse(success);
				var error = root["error_message"];
				if (error != null)
				{
					Debug.LogError("Could not retrieve geolocation for " + place + " - " + error);
					return;
				}

				var results = root["results"].AsArray;
				var result = results[0];
				var geometry = result["geometry"];
				string formatted = result["formatted_address"];
				var location = geometry["location"];
				float latitude = float.Parse(location["lat"]);
				float longitude = float.Parse(location["lng"]);
				callback(formatted, latitude, longitude);
			},
			error =>
			{
				Debug.LogError("Could not retrieve geolocation for " + place + " - " + error);
			}
		));
	}

	public static void Uptime(Action<bool, string> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://api.twitch.tv/kraken/streams/" + Config.Get(Config.STREAMER_NAME),
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
		instance.StartCoroutine(instance.PutRequest("https://api.twitch.tv/kraken/channels/" + Config.Get(Config.STREAMER_NAME), body,
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
		instance.StartCoroutine(instance.PutRequest("https://api.twitch.tv/kraken/channels/" + Config.Get(Config.STREAMER_NAME), body,
			success =>
			{
				callback(true);
			},
			error =>
			{
				callback(false);
			}));
	}

	public static void GetFollows(Action<List<string>> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://api.twitch.tv/kraken/channels/" + Config.Get(Config.STREAMER_NAME) + "/follows",
			success =>
			{
				List<string> followers = new List<string>();
				var root = JSON.Parse(success);
				var follows = root["follows"].AsArray;
				foreach (JSONNode follow in follows)
				{
					followers.Add(follow["user"]["display_name"]);
				}
				callback(followers);
			},
			error =>
			{
				Debug.LogError("Couldn't retrieve followers: " + error);
			}));
	}

	public static void GetViewers(Action<Dictionary<string, UserRole>> callback)
	{
		instance.StartCoroutine(instance.GetRequest("https://tmi.twitch.tv/group/user/" + Config.Get(Config.STREAMER_NAME) + "/chatters",
			success =>
			{
				Dictionary<string, UserRole> dict = new Dictionary<string, UserRole>();
				var root = JSON.Parse(success);
				if (root["chatter_count"].AsInt == 0)
				{
					callback(dict);
					return;
				}
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

	public static void CheckSubscription(Action<List<string>> callback)
	{
		instance.StartCoroutine(
			instance.GetRequest("https://api.twitch.tv/kraken/channels/" + Config.Get(Config.STREAMER_NAME) + "/subscriptions",
				success =>
				{
					var root = JSON.Parse(success);
					var status = root["status"];
					if (status != null)
					{
						if (status.AsInt == 422)
						{
							//No subscription program
							Debug.Log("No subscription program");
							callback(new List<string>());
							return;
						}
					}
					
					List<string> subscriptions = new List<string>();

					//TODO: Add pagination support
					var subs = root["subscriptions"].AsArray;
					foreach (JSONNode sub in subs)
					{
						var user = sub["user"];
						subscriptions.Add(user["name"]);
					}
					callback(subscriptions);
				},
				error =>
				{
					callback(new List<string>());
				},
				true
				));
	}

	private IEnumerator GetRequest(string url, Action<string> onSuccess, Action<string> onError, bool auth = false)
	{
		using (UnityWebRequest www = UnityWebRequest.Get(url))
		{
			www.SetRequestHeader("Accept", "application/vnd.twitchtv.v3+json");
			www.SetRequestHeader("Client-ID", Config.Get(Config.API_CLIENT_ID));
			if (auth)
			{
				www.SetRequestHeader("Authorization", "OAuth " + Config.Get(Config.STREAMER_TOKEN));
			}
			//www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isNetworkError)
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
			www.SetRequestHeader("Authorization", "OAuth " + Config.Get(Config.STREAMER_TOKEN));
			www.SetRequestHeader("Content-Type", "application/json");
			yield return www.Send();

			if (www.isNetworkError)
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
