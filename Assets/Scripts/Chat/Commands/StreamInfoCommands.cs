using System;

public class SetTitleCommand : ChatCommand
{
	public override string command()
	{
		return "!title";
	}

	public override void Clauses()
	{
		//TODO: With no args, return the current title
		Clause(Param.REST, (title, ctx) =>
		{
			TwitchAPI.SetTitle(title, success =>
			{
				if (success)
				{
					ctx.callback("Title updated");
				}
				else
				{
					ctx.callback("ERROR! Title not updated :(");
				}
			});
		});
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("Usage: !title TITLE");
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
		//TODO: With no args, return current game
		Clause(Param.REST, (title, ctx) =>
		{
			TwitchAPI.SetGame(title, success =>
			{
				if (success)
				{
					ctx.callback("Game updated");
				}
				else
				{
					ctx.callback("ERROR! Game not updated :(");
				}
			});
		});
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("Usage: !game GAME");
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
		return ctx =>
		{
			TwitchAPI.Uptime((success, response) =>
			{
				if (success)
				{
					if (response == null)
					{
						ctx.callback("Stream is currently offline");
					}
					else
					{
						DateTime time = DateTime.Parse(response);
						TimeSpan diff = DateTime.Now.Subtract(time);
						ctx.callback(string.Format("Uptime: {0:00}:{1:00}:{2:00}", diff.Hours, diff.Minutes, diff.Seconds));
					}
				}
				else
				{
					ctx.callback("ERROR! Couldn't retrieve uptime :(");
				}
			});
		};
	}
}