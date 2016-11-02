using System;
using System.Linq;

public class ScheduledMessageCommand : ChatCommand
{

	private const int MIN_COOLDOWN = 1;

	public override void process(Context ctx)
	{
		if (ctx.args.Length < 1)
		{
			ctx.callback("Wrong arguments :(");
			return;
		}

		if (ctx.args[0] == "list")
		{
			if (ScheduledCommandProcessor.Commands.Count == 0)
			{
				ctx.callback("No commands scheduled");
				return;
			}
			string commands = ScheduledCommandProcessor.Commands.Select(c => c.ToString()).Aggregate((cur, next) => cur + ", " + next);
			ctx.callback("Scheduled commands: " + commands);
			return;
		}

		if (ctx.args[0] == "add")
		{
			if (ctx.args.Length < 3)
			{
				ctx.callback("Usage: !scheduled add COMMAND COOLDOWN");
				return;
			}

			int cooldown;
			if (!int.TryParse(ctx.args[2], out cooldown))
			{
				ctx.callback("Cooldown should be integer");
				return;
			}

			if (cooldown < MIN_COOLDOWN)
			{
				ctx.callback("Cooldown should be at least " + MIN_COOLDOWN);
				return;
			}

			ctx.callback(ScheduledCommandProcessor.AddCommand(ctx.args[1], cooldown));
			return;
		}


		if (ctx.args[0] == "remove")
		{
			if (ctx.args.Length < 2)
			{
				ctx.callback("Usage: !scheduled remove COMMAND");
				return;
			}

			ctx.callback(ScheduledCommandProcessor.RemoveCommand(ctx.args[1]));
			return;
		}

		if (ctx.args[0] == "clear")
		{
			ctx.callback(ScheduledCommandProcessor.Clear());
			return;
		}

		ctx.callback("Unknown argument: " + ctx.args[0]);
	}

	public override string command()
	{
		return "!scheduled";
	}
}
