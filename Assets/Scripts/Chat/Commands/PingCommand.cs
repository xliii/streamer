using System;

public class PingCommand : ChatCommand {

	public override ZeroArg Default()
	{
		return c => c("pong");
	}

	public override string command()
	{
		return "!ping";
	}
}
