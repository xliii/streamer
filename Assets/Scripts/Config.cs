using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Config
{
	public const string STREAMER_NAME = "twitch.streamer";
	public const string BOT_NAME = "twitch.bot";
	//public const string IRC_CHANNEL = "twitch.irc.channel";
	public const string IRC_OAUTH = "twitch.irc.oauth";
	public const string API_CLIENT_ID = "twitch.api.client_id";
	//public const string API_CLIENT_SECRET = "twitch.api.client_secret";
	public const string API_ACCESS_TOKEN = "twitch.api.access_token";

	private static Dictionary<string, string> properties;

	public static bool Init()
	{
		return Read(File.ReadAllText("config.txt"));
	}

	private static bool Read(string config)
	{
		properties = new Dictionary<string, string>();
		string[] lines = config.Split('\n');
		foreach (string line in lines)
		{
			string[] property = line.Split('=');
			if (property.Length != 2)
			{
				Debug.LogError("Could not parse property from line: " + line);
				return false;
			}
			var value = property[1].Trim().ToLower();
			if (value == "") return false;
			
			properties[property[0]] = value;
		}

		properties[API_CLIENT_ID] = "9r82dk02lbj7cdk5ln8fkxzqnzvt4ic";
		return true;
	}

	public static string Get(string property)
	{
		if (properties == null)
		{
			Init();
		}

		if (!properties.ContainsKey(property))
		{
			Debug.LogError("No property found: " + property);
			return "";
		}

		return properties[property];
	}
}
