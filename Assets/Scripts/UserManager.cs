using System;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	public static UserManager instance;

	const int ONLINE_UPDATE_RATE = 60000; //60 seconds
	System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

	List<User> online = new List<User>();

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		Subcribers();
		UpdateOnlineUsers(true);
		stopWatch.Start();
	}


	// Update is called once per frame
	void Update ()
	{
		CheckOnline();
	}

	void Subcribers()
	{
		TwitchAPI.CheckSubscription(subs =>
		{
			int newSubs = 0;
			foreach (string username in subs)
			{
				User sub = UserRepository.GetByUsername(username);
				if (sub == null)
				{
					sub = Register(username, UserRole.Viewer);
				}
				if (sub.AddRole(UserRole.Subscriber))
				{
					newSubs++;
				}
				UserRepository.Save(sub);
			}
			if (newSubs > 0)
			{
				Debug.LogFormat("Added {0} new subs", newSubs);
			}
		});
	}

	//TODO: Don't tie online check to launch time
	void UpdateOnlineUsers(bool initial = false)
	{
		online.Clear();
		TwitchAPI.GetViewers(users =>
		{
			foreach (string username in users.Keys)
			{
				var user = UserRepository.GetByUsername(username);
				if (user == null)
				{
					user = Register(username, users[username]);
				}

				user.lastOnline = DateTime.Now;
				online.Add(user);
				if (initial) continue;

				user.AddOnlinePoints();
				UserRepository.Save(user);
			}
		});
	}

	public User Register(string username, UserRole role)
	{
		var user = new User(username);
		user.AddRole(role);
		UserRepository.Save(user);
		return user;
	}

	public User GetBot()
	{
		return UserRepository.GetByUsername(Config.Get(Config.BOT_NAME));
	}

	void CheckOnline()
	{
		if (stopWatch.ElapsedMilliseconds > ONLINE_UPDATE_RATE)
		{
			UpdateOnlineUsers();
			stopWatch.Reset();
			stopWatch.Start();
		}
	}
}
