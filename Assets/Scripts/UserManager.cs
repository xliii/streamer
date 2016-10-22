using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserManager : MonoBehaviour
{
	private const int ONLINE_UPDATE_RATE = 60000; //60 seconds
	System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
	List<User> allUsers = new List<User>();
	List<User> online = new List<User>();

	void Start()
	{
		//TODO: retrieve all users
		UpdateOnlineUsers();
		stopWatch.Start();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckOnline();
	}

	void UpdateOnlineUsers()
	{
		online.Clear();
		TwitchAPI.GetViewers(users =>
		{
			foreach (string username in users.Keys)
			{
				if (!KnownUser(username))
				{
					//register
					//Debug.Log("Register new user: " + username);
					var newUser = new User(username);
					newUser.SetRole(users[username]);
					newUser.lastOnline = DateTime.Now;
					allUsers.Add(newUser);
				}

				var user = GetUser(username);
				if (user == null)
				{
					Debug.LogError("Couldn't find user: " + username + " THIS SHOULD NOT HAPPEN");
					continue;
				}

				online.Add(user);
				user.AddOnlinePoints();
			}
		});
	}

	bool KnownUser(string username)
	{
		return allUsers.Any(user => user.username.ToLower() == username.ToLower());
	}

	User GetUser(string username)
	{
		return allUsers.Find(user => user.username.ToLower() == username.ToLower());
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
