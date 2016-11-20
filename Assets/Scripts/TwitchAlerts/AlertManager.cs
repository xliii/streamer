using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
	private Queue<AlertData> queue = new Queue<AlertData>();

	public FollowerAlert[] followerAlerts;

	public DonationAlert[] donationsAlerts;

	public HostAlert[] hostAlerts;

	public static bool alertInProgress;	

	// Use this for initialization
	void Awake()
	{
		//DontDestroyOnLoad(gameObject);
	}

	void Start () {
		Messenger.AddListener<AlertData, bool>(TwitchAlertsType.most_recent_follower.ToString(), OnFollowerAlert);
		Messenger.AddListener<DonationAlertData>(TwitchAlertsType.most_recent_donator.ToString(), OnDonationAlert);
		Messenger.AddListener<AlertData>(TwitchAlertsType.host_alert.ToString(), OnHostAlert);
	}

	void OnHostAlert(AlertData data)
	{
		Debug.Log("Host Alert: " + data.username + " | " + ((HostAlertData) data).viewers);
		queue.Enqueue(data);
	}

	void OnDonationAlert(DonationAlertData data)
	{
		Debug.Log("Donation Alert: " + data.username + " | " + data.amountFormatted);
		queue.Enqueue(data);
	}

	void OnFollowerAlert(AlertData data, bool init)
	{
		if (init)
		{
			//SetLayoutText(data);
			return;
		}
		Debug.Log("Follower Alert: " + data.username);

		queue.Enqueue(data);
	}

	void Update()
	{
		if (queue.Count > 0)
		{
			if (alertInProgress) return;

			AlertData data = queue.Dequeue();
			Alert processor = GetProcessor(data);
			if (processor == null)
			{
				Debug.LogError("No processor found for " + data.type + " | " + data.username);
				return;
			}

			processor.Process(data);
		}
	}

	Alert GetProcessor(AlertData data)
	{
		switch (data.type)
		{
			case TwitchAlertsType.most_recent_donator:
				DonationAlertData donationData = data as DonationAlertData;
				return GetDonationAlertProcessor(donationData.amount);
			case TwitchAlertsType.most_recent_follower:
				return GetFollowerAlertProcessor();
			case TwitchAlertsType.host_alert:
				return GetHostAlertProcessor();
			default:
				return null;
		}
	}

	FollowerAlert GetFollowerAlertProcessor()
	{
		return followerAlerts[Random.Range(0, followerAlerts.Length)];
	}

	DonationAlert GetDonationAlertProcessor(float amount)
	{
		DonationAlert[] candidates = donationsAlerts.Where(alert => alert.Matches(amount)).ToArray();

		if (candidates.Length == 0)
		{
			return donationsAlerts[Random.Range(0, donationsAlerts.Length)];
		}

		return candidates[Random.Range(0, candidates.Length)];
	}

	HostAlert GetHostAlertProcessor()
	{
		return hostAlerts[Random.Range(0, hostAlerts.Length)];
	}
}
