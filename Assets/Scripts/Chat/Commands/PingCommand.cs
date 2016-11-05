using System;

public class PingCommand : ChatCommand {

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("pong");
	}

	public override string command()
	{
		return "!ping";
	}
}
