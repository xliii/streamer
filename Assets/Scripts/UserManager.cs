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
		UpdateOnlineUsers(true);
		stopWatch.Start();
	}


	// Update is called once per frame
	void Update ()
	{
		CheckOnline();
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
		return UserRepository.GetByUsername(Config.BOT_NAME);
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
