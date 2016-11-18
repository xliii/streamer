using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowManager : MonoBehaviour {

	const int FOLLOW_UPDATE_RATE = 30000; //30 seconds

	System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

	private List<string> lastFollowers = new List<string>(); //25 by default

	void Start()
	{
		PollFollowers();
		stopWatch.Start();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckFollows();
	}

	void PollFollowers()
	{
		Debug.Log("Polling for followers");
		TwitchAPI.GetFollows(followers =>
		{
			if (lastFollowers.Count == 0)
			{
				lastFollowers = followers;
				return;
			}

			int newFollowers = 0;
			foreach (var follower in followers)
			{
				if (lastFollowers.Contains(follower)) continue;

				newFollowers++;
				Messenger.Broadcast(TwitchAlertsType.most_recent_follower.ToString(), FollowAlertData.Create(follower), false);
			}

			if (newFollowers > 0)
			{
				Debug.Log("New followers: " + newFollowers);
			}
			lastFollowers = followers;
		});
	}

	void CheckFollows()
	{
		if (stopWatch.ElapsedMilliseconds > FOLLOW_UPDATE_RATE)
		{
			PollFollowers();
			stopWatch.Reset();
			stopWatch.Start();
		}
	}
}
