using System.Collections.Generic;

public class User {

	public const string ROLE_STREAMER = "ROLE_STREAMER";
	public const string ROLE_MOD = "ROLE_MOD";

	public string nick;
	public List<string> roles;

	public User(string name)
	{
		this.nick = name;
		if (name.ToLower() == "xliii")
		{
			roles = new List<string>();
			roles.Add(ROLE_STREAMER);
		}
	}

	public bool HasRole(string role)
	{
		return roles != null && roles.Contains(role);
	}
	
	public bool HasAnyRole(string[] roles)
	{
		if (this.roles == null || this.roles.Count == 0) return false;

		foreach (string role in roles)
		{
			if (this.roles.Contains(role)) return true;
		}
		return false;
	}	
}
