using System;

public class SetTitleCommand : ChatCommand
{
	public override string command()
	{
		return "!title";
	}

	public override void process(string user, string[] args, Action<string> callback)
	{
		if (args.Length == 0)
		{
			callback("Please specify title");
			return;
		}

		string title = string.Join(" ", args);
		TwitchAPI.SetTitle(title, callback);
	}

	public override bool hide()
	{
		return true;
	}

	public override string[] roles()
	{
		return new string[] { User.ROLE_STREAMER };
	}
}

public class SetGameCommand : ChatCommand
{
	public override string command()
	{
		return "!game";
	}

	public override void process(string user, string[] args, Action<string> callback)
	{
		if (args.Length == 0)
		{
			callback("Please specify game");
			return;
		}

		string game = string.Join(" ", args);
		TwitchAPI.SetGame(game, callback);
	}

	public override bool hide()
	{
		return true;
	}

	public override string[] roles()
	{
		return new string[] { User.ROLE_STREAMER };
	}
}

public class UptimeCommand : ChatCommand
{
	public override string command()
	{
		return "!uptime";
	}

	public override void process(string user, string[] args, Action<string> callback)
	{
		TwitchAPI.Uptime(callback);
	}
	
}