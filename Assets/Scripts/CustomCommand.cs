using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomCommand : ChatCommand
{
	public string commandName = "";
	

	public override string command()
	{
		return commandName;
	}

	public override void process(User user, string[] args, Action<string> callback)
	{
		
	}

	public override bool hide()
	{
		return true;
	}
}
