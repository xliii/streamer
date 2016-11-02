using System;

public class SetTitleCommand : ChatCommand
{
	public override string command()
	{
		return "!title";
	}

	public override void Clauses()
	{
		Clause(Keyword.REST, (title, callback) =>
		{
			TwitchAPI.SetTitle(title, success =>
			{
				if (success)
				{
					callback("Title updated");
				}
				else
				{
					callback("ERROR! Title not updated :(");
				}
			});
		});
	}

	public override ZeroArg Default()
	{
		return c => c("Usage: !title TITLE");
	}

	public override bool hide()
	{
		return true;
	}

	public override UserRole[] roles()
	{
		return new UserRole[] { UserRole.Streamer };
	}
}

public class SetGameCommand : ChatCommand
{
	public override string command()
	{
		return "!game";
	}

	public override void Clauses()
	{
		Clause(Keyword.REST, (title, callback) =>
		{
			TwitchAPI.SetGame(title, success =>
			{
				if (success)
				{
					callback("Game updated");
				}
				else
				{
					callback("ERROR! Game not updated :(");
				}
			});
		});
	}

	public override ZeroArg Default()
	{
		return c => c("Usage: !game GAME");
	}

	public override bool hide()
	{
		return true;
	}

	public override UserRole[] roles()
	{
		return new UserRole[] { UserRole.Streamer };
	}
}

public class UptimeCommand : ChatCommand
{
	public override string command()
	{
		return "!uptime";
	}

	public override ZeroArg Default()
	{
		return callback =>
		{
			TwitchAPI.Uptime((success, response) =>
			{
				if (success)
				{
					if (response == null)
					{
						callback("Stream is currently offline");
					}
					else
					{
						DateTime time = DateTime.Parse(response);
						TimeSpan diff = DateTime.Now.Subtract(time);
						callback(string.Format("Uptime: {0:00}:{1:00}:{2:00}", diff.Hours, diff.Minutes, diff.Seconds));
					}
				}
				else
				{
					callback("ERROR! Couldn't retrieve uptime :(");
				}
			});
		};
	}
}