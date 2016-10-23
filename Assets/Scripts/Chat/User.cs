using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class User {

	public string username;
	public List<UserRole> roles = new List<UserRole>();
	public DateTime lastOnline;

	[SerializeField]
	private float points;

	public int Points
	{
		get { return (int) points; }
	}

	public User(string name)
	{
		username = name.ToLower();
		if (username == Config.Get(Config.STREAMER_NAME))
		{
			AddRole(UserRole.Streamer);
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
	
	public bool HasAnyRole(UserRole[] requiredRoles)
	{
		if (requiredRoles.Length == 0) return true;

		if (roles.Count == 0) return false;

		return requiredRoles.Any(role => roles.Contains(role));
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

	public void AddRole(UserRole role)
	{
		if (role == UserRole.None) return;

		if (!HasRole(role))
		{
			roles.Add(role);
		}
	}

	public static bool operator ==(User a, User b)
	{
		if (ReferenceEquals(a, b))
		{
			return true;
		}

		if ((object)a == null || (object)b == null)
		{
			return false;
		}

		return a.username == b.username;

	}

	public static bool operator !=(User a, User b)
	{
		return !(a == b);
	}

	protected bool Equals(User other)
	{
		return string.Equals(username, other.username);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((User) obj);
	}

	public override int GetHashCode()
	{
		return (username != null ? username.GetHashCode() : 0);
	}
}
