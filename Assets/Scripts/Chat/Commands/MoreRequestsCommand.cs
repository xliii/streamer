using System;

public class MoreRequestsCommand : ChatCommand
{
	public override string command()
	{
		return "!more";
	}

	public override void process(string user, string[] args, Action<string> callback)
	{
		callback("SMOrc WE NEED MORE REQUESTS! SMOrc");
	}
}
