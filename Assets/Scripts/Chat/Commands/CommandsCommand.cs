using System;
using System.Linq;

public class CommandsCommand : ChatCommand {

	public override void process(User user, string[] args, Action<string> callback)
	{
		string commands = TwitchIRCProcessor.commands.Where(cmd => !cmd.hide())
			.Aggregate("", (current, cmd) => current + ", " + cmd.command());
		callback("Available commands: " + commands);
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
