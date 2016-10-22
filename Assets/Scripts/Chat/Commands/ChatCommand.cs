using UnityEngine;
using System;

public abstract class ChatCommand : ScriptableObject {

	public abstract void process(User user, string[] args, Action<string> callback);

	public abstract string command();

	public virtual bool hide()
	{
		return false;
	}

	public virtual UserRole[] roles()
	{
		return new UserRole[0];
	}

	public virtual int cooldown()
	{
		return 0;
	}
}
