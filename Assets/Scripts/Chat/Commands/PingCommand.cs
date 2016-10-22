using System;

public class PingCommand : ChatCommand {

	public override void process(User user, string[] args, Action<string> callback)
	{
		callback("pong");
	}

	public override string command()
	{
		return "!ping";
	}
}
