using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class User {

	public string username;
	public HashSet<UserRole> roles = new HashSet<UserRole>();
	public DateTime lastOnline;

	public float points;

	public User(string name)
	{
		username = name;
		if (name.ToLower() == "xliii")
		{
			roles.Add(UserRole.Streamer);
		}
	}

	public bool HasRole(UserRole role)
	{
		return roles.Contains(role);
	}

	float PointsMultiplier()
	{
		return roles.Max(role => role.pointsMultiplier());
	}
	
	public bool HasAnyRole(UserRole[] roles)
	{
		if (roles.Length == 0) return true;

		if (this.roles.Count == 0) return false;

		return roles.Any(role => this.roles.Contains(role));
	}

	public void AddOnlinePoints()
	{
		var toAdd = PointsMultiplier();
		Debug.Log(string.Format("Adding {0} points to {1}", toAdd, username));
		points += toAdd;
	}

	public void SetRole(UserRole role)
	{
		roles.Add(role);
	}
}
