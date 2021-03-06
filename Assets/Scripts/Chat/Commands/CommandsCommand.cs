﻿using System.Linq;

public class CommandsCommand : ChatCommand {
	public override ZeroArg Default()
	{
		return ctx =>
		{
			string commands = TwitchIRCProcessor.commands.Where(cmd => !cmd.hide())
			.Aggregate("", (current, cmd) => current + ", " + cmd.command());
			ctx.callback("Available commands: " + commands);
		};
	}

	public override string command()
	{
		return "!commands";
	}

	public override bool hide()
	{
		return true;
	}
}
