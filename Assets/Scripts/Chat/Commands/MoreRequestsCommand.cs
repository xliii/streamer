using System;

public class MoreRequestsCommand : ChatCommand
{
	public override string command()
	{
		return "!more";
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("SMOrc WE NEED MORE REQUESTS! SMOrc");
	}
}
