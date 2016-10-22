using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class User {

	public string username;
	public HashSet<UserRole> roles = new HashSet<UserRole>();
	public DateTime lastOnline;

	private float points;

	public int Points
	{
		get { return (int) points; }
	}

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

	public void AddPoints(float amount)
	{
		points += amount;
	}

	public bool RemovePoints(float amount)
	{
		if (points < amount) return false;

		points -= amount;
		return true;
	}

	public void AddOnlinePoints()
	{
		AddPoints(PointsMultiplier());
	}

	public void SetRole(UserRole role)
	{
		roles.Add(role);
	}
}
