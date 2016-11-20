using System;
using System.Linq;
using UnityEngine;

public class AlertData
{
	public TwitchAlertsType type;
	public string username;

	protected AlertData() {}

	public static AlertData Parse(TwitchAlertsType type, string data)
	{
		switch (type)
		{
			case TwitchAlertsType.most_recent_donator:
				string[] entries = data.Split(new[] { TextFromFile.DELIMETER }, StringSplitOptions.None);
				string[] left = entries[0].Split('(');

				string amountStr = new string(left[1].Where(c => char.IsDigit(c) || c == '.').ToArray());
				float amount;
				if (!float.TryParse(amountStr, out amount))
				{
					amount = 0;
					Debug.LogError("Could not parse amount: " + amountStr);
				}

				return new DonationAlertData()
				{
					type = TwitchAlertsType.most_recent_donator,
					username = left[0].Trim(),
					amount = amount,
					message = entries[1]
				};
			case TwitchAlertsType.most_recent_follower:
				return new AlertData()
				{
					type = TwitchAlertsType.most_recent_follower,
					username = data.Trim()
				};
			default:
				Debug.LogError("Cannot parse AlertData: " + type + " | " + data);
				return null;
		}
	}
}

public class FollowAlertData : AlertData
{
	public static AlertData Create(string username)
	{
		return new FollowAlertData()
		{
			type = TwitchAlertsType.most_recent_follower,
			username = username
		};
	}
}

public class DonationAlertData : AlertData
{
	public float amount;
	public string message;
	public int timestamp;

	public static AlertData Create(string username, float amount, string message, int timestamp = 0)
	{
		return new DonationAlertData()
		{
			type = TwitchAlertsType.most_recent_donator,
			username = username,
			amount = amount,
			message = message,
			timestamp = timestamp
		};
	}

	public string amountFormatted
	{
		get { return string.Format("€{0:F}", amount); }
	}
}

public class HostAlertData : AlertData
{
	public int viewers;

	public static AlertData Create(string username, int viewers)
	{
		return new HostAlertData()
		{
			type = TwitchAlertsType.host_alert,
			username = username,
			viewers = viewers
		};
	}
}
