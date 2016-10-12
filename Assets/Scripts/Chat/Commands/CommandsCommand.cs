public class CommandsCommand : ChatCommand {

	public override string process(string user, string[] args)
	{
		string cmds = "Available commands: ";
		foreach (ChatCommand cmd in TwitchIRCProcessor.commands)
		{
			if (cmd.hide()) continue;
			cmds += cmd.command() + ", ";
		}
		return cmds.Substring(0, cmds.Length - 2);
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
