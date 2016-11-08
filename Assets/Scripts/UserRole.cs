using System.Collections.Generic;
using System.Linq;

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
	private static string[] subAliases = { "sub", "subscriber", "subs" };
	private static string[] modAliases = { "mod", "mods", "moderator", "op" };
	private static string[] viewerAliases = { "viewer", "viewers", "pleb", "plebs" };
	private static string[] streamerAliases = { "streamer", "me" };
	private static string[] staffAliases = { "staff" };
	private static string[] adminAliases = { "admin" };
	private static string[] globalModAliases = { "globalmod", "globalmoderator", "global" };
	private static string[] botAliases = { "bot" };

	public static UserRole Parse(string role)
	{
		if (subAliases.Contains(role.ToLower()))
		{
			return UserRole.Subscriber;
		}
		if (modAliases.Contains(role.ToLower()))
		{
			return UserRole.Mod;
		}
		if (viewerAliases.Contains(role.ToLower()))
		{
			return UserRole.Viewer;
		}
		if (streamerAliases.Contains(role.ToLower()))
		{
			return UserRole.Streamer;
		}
		if (staffAliases.Contains(role.ToLower()))
		{
			return UserRole.Staff;
		}
		if (adminAliases.Contains(role.ToLower()))
		{
			return UserRole.Admin;
		}
		if (globalModAliases.Contains(role.ToLower()))
		{
			return UserRole.GlobalMod;
		}
		if (botAliases.Contains(role.ToLower()))
		{
			return UserRole.Bot;
		}

		return UserRole.None;
	}

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
