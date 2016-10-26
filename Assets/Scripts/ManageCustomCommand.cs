using System;
using System.Collections.Generic;
using System.Linq;

public class ManageCustomCommand : ChatCommand {

	//-- !custom list --

	//-- !custom add COMMAND --
	//!custom add twitter

	//-- !custom add COMMAND RESPONSE --
	//!custom add twitter www.twitter.com/followxlii
	//!custom add twitter "plz follow me at www.twitter.com/followxliii"

	//-- !custom remove COMMAND --
	//!custom remove twitter

	//-- !custom addoption COMMAND RESPONSE --
	//!custom add randomABC
	//!custom addoption randomABC A
	//!custom addoption randomABC B
	//!custom addoption randomABC C

	//-- !custom setrole COMMAND ROLE --
	//!custom setrole more mod

	private Dictionary<string, string> cmds = new Dictionary<string, string>();

	public override string command()
	{
		return "!custom";
	}

	public override void process(User user, string[] args, Action<string> callback)
	{
		if (args.Length < 1)
		{
			//TODO: Fix usage message
			callback("USAGE: !custom list");
			return;
		}

		if (args[0] == "list")
		{
			if (cmds.Count == 0)
			{
				callback("No custom commands registered");
				return;
			}
			callback(string.Join(", ", cmds.Keys.ToArray()));
			return;
		}

		if (args.Length < 2)
		{
			callback("USAGE: !custom add COMMAND");
			return;
		}

		string command = args[1];

		if (args[0] == "remove")
		{
			if (!cmds.Keys.Contains(command))
			{
				callback("No such custom command: " + Normalize(command));
				return;
			}

			cmds.Remove(command);
			callback("Custom command removed");
			return;
		}

		if (args[0] == "add")
		{
			string response = args.Length > 2 ? args[2] : "Default response";
			cmds[command] = response;
			callback("Custom command added");
			return;
		}

		callback("Unknown options: " + string.Join(" ", args));
	}
}
