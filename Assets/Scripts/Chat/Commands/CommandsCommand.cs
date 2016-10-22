using System;

public class CommandsCommand : ChatCommand {

	public override void process(User user, string[] args, Action<string> callback)
	{
		string cmds = "Available commands: ";
		foreach (ChatCommand cmd in TwitchIRCProcessor.commands)
		{
			if (cmd.hide()) continue;
			cmds += cmd.command() + ", ";
		}
		callback(cmds.Substring(0, cmds.Length - 2));
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
