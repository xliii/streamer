﻿using UnityEngine;

public abstract class ChatCommand : ScriptableObject {

	public abstract string process(string user, string[] args);

	public abstract string command();

	public virtual bool hide()
	{
		return false;
	}
}
