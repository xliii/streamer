using UnityEngine;
using System;

public abstract class ChatCommand : ScriptableObject {

	public abstract void process(string user, string[] args, Action<string> callback);

	public abstract string command();

	public virtual bool hide()
	{
		return false;
	}

	public virtual string[] roles()
	{
		return new string[0];
	}
}
