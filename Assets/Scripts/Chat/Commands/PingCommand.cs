public class PingCommand : ChatCommand {

	public override string process(string user, string[] args)
	{
		return "pong";
	}

	public override string command()
	{
		return "!ping";
	}
}
