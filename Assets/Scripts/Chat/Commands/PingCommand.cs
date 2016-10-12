using System;

public class PingCommand : ChatCommand {

	public override void process(string user, string[] args, Action<string> callback)
	{
		callback("pong");
	}

	public override string command()
	{
		return "!ping";
	}
}
