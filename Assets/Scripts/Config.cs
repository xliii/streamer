using System.Collections.Generic;
using UnityEngine;

public static class Config
{
	public const string IRC_NICK = "twitch.irc.nick";
	public const string IRC_CHANNEL = "twitch.irc.channel";
	public const string IRC_OAUTH = "twitch.irc.oauth";
	public const string API_CLIENT_ID = "twitch.api.client_id";
	public const string API_CLIENT_SECRET = "twitch.api.client_secret";
	public const string API_ACCESS_TOKEN = "twitch.api.access_token";

	private static Dictionary<string, string> properties;

	public static void Init()
	{
		properties = new Dictionary<string, string>();
		TextAsset config = Resources.Load<TextAsset>("config");
		if (config == null)
		{
			Debug.LogError("Unable to read config: \"config.txt\" missing from Resources/");
			return;
		}

		string[] lines = config.text.Split('\n');
		foreach (string line in lines)
		{
			string[] property = line.Split('=');
			if (property.Length != 2)
			{
				Debug.LogError("Could not parse property from line: " + line);
				continue;
			}

			properties[property[0]] = property[1].Trim().ToLower();
		}
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
