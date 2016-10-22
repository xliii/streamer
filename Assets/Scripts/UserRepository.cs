using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserRepository {
	
	const string PREFS_USER = "PREFS_USER_{0}";

	public static void Save(User user)
	{
		PlayerPrefs.SetString(PrefsUser(user.username), JsonUtility.ToJson(user));
	}
	
	public static User GetByUsername(string username)
	{
		var json = PlayerPrefs.GetString(PrefsUser(username));
		return JsonUtility.FromJson<User>(json);
	}

	static string PrefsUser(string username)
	{
		return string.Format(PREFS_USER, username);
	}

}
