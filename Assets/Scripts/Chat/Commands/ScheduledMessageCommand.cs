using System;
using System.Linq;
using UnityEngine;

public class ScheduledMessageCommand : ChatCommand
{
	private const int MIN_COOLDOWN = 1;

	public override UserRole[] roles()
	{
		return new UserRole[] { UserRole.Streamer };
	}

	public override void Clauses()
	{
		Clause("list", ctx =>
		{
			if (ScheduledCommandProcessor.Commands.Count == 0)
			{
				ctx.callback("No commands scheduled");
				return;
			}
			string commands = ScheduledCommandProcessor.Commands.Select(c => c.ToString()).Aggregate((cur, next) => cur + ", " + next);
			ctx.callback("Scheduled commands: " + commands);
		});

		Clause("add COMMAND COOLDOWN", (command, cd, ctx) =>
		{
			Debug.Log(command + " " + cd);
			int cooldown;
			if (!int.TryParse(cd, out cooldown))
			{
				ctx.callback("Cooldown should be integer");
				return;
			}

			if (cooldown < MIN_COOLDOWN)
			{
				ctx.callback("Cooldown should be at least " + MIN_COOLDOWN);
				return;
			}

			ctx.callback(ScheduledCommandProcessor.AddCommand(command, cooldown));
		});
		var invalidAdd = "Usage: !scheduled add COMMAND COOLDOWN";
		Clause("add COMMAND", invalidAdd);
		Clause("add", invalidAdd);

		Clause("clear", ctx => ctx.callback(ScheduledCommandProcessor.Clear()));
		
		Clause("remove COMMAND", (command, ctx) =>
		{
			ctx.callback(ScheduledCommandProcessor.RemoveCommand(command));
		});
		Clause("remove", "Usage: !scheduled remove COMMAND");
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("Bad arguments");
	}

	public override string command()
	{
		return "!scheduled";
	}
}
