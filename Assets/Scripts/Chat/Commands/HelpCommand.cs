using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCommand : ChatCommand {
	public override string command()
	{
		return "!help";
	}

	//TODO: Provide command option descriptions to PM, give a link to API
	public override void Clauses()
	{
		Clause("COMMAND", (command, ctx) =>
		{
			string cmdName = Normalize(command);
			foreach (ChatCommand cmd in TwitchIRCProcessor.commands)
			{
				if (cmd.command() != cmdName) continue;

				foreach (CommandClause clause in cmd.clauses)
				{
					Debug.Log(clause.Option() + " -> " + clause.Description());
				}
				ctx.callback("This command is not complete yet :D");
				return;
			}
			ctx.callback("No such command: " + cmdName);
		});
	}

	public override bool hide()
	{
		return true;
	}
}
