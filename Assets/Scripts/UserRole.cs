using System;
using System.Collections;
using System.Collections.Generic;

public enum UserRole {

	Streamer,
	Bot,
	Mod,
	Staff,
	Subscriber,
	Admin,
	GlobalMod,
	Viewer,
	None
}

public static class UserRoleExtensions
{
	public static float PointsMultiplier(this UserRole role)
	{
		if (role == UserRole.Subscriber) return 2f;

		return 1f;
	}

	public static int Priority(this UserRole role)
	{
		switch (role)
		{
			case UserRole.Streamer:
				return 43;
			case UserRole.Bot:
				return 40;
			case UserRole.Mod:
				return 20;
			case UserRole.Staff:
				return 12;
			case UserRole.Admin:
				return 11;
			case UserRole.GlobalMod:
				return 10;
			case UserRole.Subscriber:
				return 5;
			case UserRole.Viewer:
				return 1;
			case UserRole.None:
				return 0;
			default:
				return 2;
		}
	}
}

public class UserRoleComparer : IComparer<UserRole>
{
	public int Compare(UserRole x, UserRole y)
	{
		return y.Priority() - x.Priority();
	}
}
