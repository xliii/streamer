public class SetTitleCommand : ChatCommand
{
	public override string command()
	{
		return "!title";
	}

	public override string process(string user, string[] args)
	{
		if (args.Length == 0)
		{
			return "Please specify title";
		}

		string title = string.Join(" ", args);
		TwitchAPI.SetTitle(title);
		return "Title updated";
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

	public override string process(string user, string[] args)
	{
		if (args.Length == 0)
		{
			return "Please specify game";
		}

		string game = string.Join(" ", args);
		TwitchAPI.SetGame(game);
		return "Game updated";
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