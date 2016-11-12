using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomCommand : ChatCommand
{
	public string commandName = "!customcustom";

	public override string command()
	{
		return commandName;
	}

	public override void process(Context context)
	{
		context.callback("DO LITERALLY NOTHING");
	}

	public override bool hide()
	{
		return true;
	}
}
