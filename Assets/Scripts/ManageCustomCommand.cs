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

	public override void process(Context ctx)
	{
		if (ctx.args.Length < 1)
		{
			//TODO: Fix usage message
			ctx.callback("USAGE: !custom list");
			return;
		}

		if (ctx.args[0] == "list")
		{
			if (cmds.Count == 0)
			{
				ctx.callback("No custom commands registered");
				return;
			}
			ctx.callback(string.Join(", ", cmds.Keys.ToArray()));
			return;
		}

		if (ctx.args.Length < 2)
		{
			ctx.callback("USAGE: !custom add COMMAND");
			return;
		}

		string command = ctx.args[1];

		if (ctx.args[0] == "remove")
		{
			if (!cmds.Keys.Contains(command))
			{
				ctx.callback("No such custom command: " + Normalize(command));
				return;
			}

			cmds.Remove(command);
			ctx.callback("Custom command removed");
			return;
		}

		if (ctx.args[0] == "add")
		{
			string response = ctx.args.Length > 2 ? ctx.args[2] : "Default response";
			cmds[command] = response;
			ctx.callback("Custom command added");
			return;
		}

		ctx.callback("Unknown options: " + string.Join(" ", ctx.args));
	}
}
