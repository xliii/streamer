using System;
using System.Linq;
using UnityEngine;

public class AlertData
{
	public TwitchAlertsType type;
	public string username;

	protected AlertData()
	{
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
	public int id;

	public static DonationAlertData Create(string username, float amount, string message, int id = 0)
	{
		return new DonationAlertData()
		{
			type = TwitchAlertsType.most_recent_donator,
			username = username,
			amount = amount,
			message = message,
			id = id
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
