using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlagRepository
{

	private class FlagUsersWrapper
	{
		public List<string> flagUsers;

		public FlagUsersWrapper(List<string> flagUsers)
		{
			this.flagUsers = flagUsers;
		}
	}

	static HashSet<Flag> flags;
	static HashSet<string> flagUsers;

	//TODO: Store single wrapper

	const string PREFS_FLAG = "PREFS_FLAG_{0}";
	const string PREFS_FLAG_USERS = "PREFS_FLAG_USERS";

	public static void Save(Flag flag)
	{
		if (flags != null)
		{
			if (flags.Add(flag))
			{
				Messenger.Broadcast(Flag.FLAG_ADDED, flag);
			}
		}

		PlayerPrefs.SetString(string.Format(PREFS_FLAG, flag.user), JsonUtility.ToJson(flag));
		if (FlagUsers.Add(flag.user))
		{
			PlayerPrefs.SetString(PREFS_FLAG_USERS, JsonUtility.ToJson(new FlagUsersWrapper(FlagUsers.ToList())));
		}
	}

	public static int Clear()
	{
		var all = GetAll();
		int count = all.Count;
		foreach (Flag flag in all)
		{
			PlayerPrefs.DeleteKey(string.Format(PREFS_FLAG, flag.user));
		}
		PlayerPrefs.DeleteKey(PREFS_FLAG_USERS);
		Messenger.Broadcast(Flag.FLAGS_CLEARED);
		all.Clear();
		flagUsers.Clear();
		return count;
	}

	private static HashSet<string> FlagUsers
	{
		get
		{
			if (flagUsers == null)
			{
				var json = PlayerPrefs.GetString(PREFS_FLAG_USERS, "{}");
				var wrapper = JsonUtility.FromJson<FlagUsersWrapper>(json);
				flagUsers = new HashSet<string>(wrapper.flagUsers);
			}
			return flagUsers;
		}
	}

	public static bool DeleteByUsername(string username)
	{
		Flag flag = GetByUsername(username);
		if (flag == null)
		{
			return false;
		}

		if (flags != null)
		{
			flags.Remove(flag);
		}

		PlayerPrefs.DeleteKey(string.Format(PREFS_FLAG, flag.user));
		if (FlagUsers.Remove(flag.user))
		{
			PlayerPrefs.SetString(PREFS_FLAG_USERS, JsonUtility.ToJson(new FlagUsersWrapper(FlagUsers.ToList())));
		}
		return true;
	}

	public static Flag GetByUsername(string username)
	{
		var json = PlayerPrefs.GetString(string.Format(PREFS_FLAG, username));
		return JsonUtility.FromJson<Flag>(json);
	}	

	public static ICollection<Flag> GetAll()
	{
		if (flags == null)
		{
			flags = new HashSet<Flag>();
			foreach (string flagUser in FlagUsers)
			{
				Flag flag = GetByUsername(flagUser);
				if (flag == null)
				{
					Debug.LogError("Couldn't retrieve flag for: " + flagUser);
					continue;
				}
				
				flags.Add(flag);
			}
		}
		return flags;
	} 
}

public class Flag
{
	public const string FLAG_ADDED = "flagAdded";
	public const string FLAG_REMOVED = "flagRemoved";
	public const string FLAG_MODIFIED = "flagModified";
	public const string FLAGS_CLEARED = "flagsCleared";	

	public string user;
	public string place;
	public float latitude;
	public float longitude;	

	public Flag(string username)
	{
		this.user = username;
	}

	#region Equals

	public static bool operator ==(Flag a, Flag b)
	{
		if (ReferenceEquals(a, b))
		{
			return true;
		}

		if ((object)a == null || (object)b == null)
		{
			return false;
		}

		return a.user == b.user;

	}

	public static bool operator !=(Flag a, Flag b)
	{
		return !(a == b);
	}

	protected bool Equals(Flag other)
	{
		return string.Equals(user, other.user);
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((Flag)obj);
	}

	public override int GetHashCode()
	{
		return (user != null ? user.GetHashCode() : 0);
	}

	#endregion
}
