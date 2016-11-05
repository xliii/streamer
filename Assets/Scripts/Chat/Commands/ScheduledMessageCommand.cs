using System;
using System.Linq;
using UnityEngine;

public class ScheduledMessageCommand : ChatCommand
{
	private const int MIN_COOLDOWN = 1;

	public override void Clauses()
	{
		Clause("list", callback =>
		{
			if (ScheduledCommandProcessor.Commands.Count == 0)
			{
				callback("No commands scheduled");
				return;
			}
			string commands = ScheduledCommandProcessor.Commands.Select(c => c.ToString()).Aggregate((cur, next) => cur + ", " + next);
			callback("Scheduled commands: " + commands);
		});
		var invalidAdd = "Usage: !scheduled add COMMAND COOLDOWN";

		Clause("add COMMAND COOLDOWN", (command, cd, callback) =>
		{
			Debug.Log(command + " " + cd);
			int cooldown;
			if (!int.TryParse(cd, out cooldown))
			{
				callback("Cooldown should be integer");
				return;
			}

			if (cooldown < MIN_COOLDOWN)
			{
				callback("Cooldown should be at least " + MIN_COOLDOWN);
				return;
			}

			callback(ScheduledCommandProcessor.AddCommand(command, cooldown));
		});
		Clause("add COMMAND", invalidAdd);
		Clause("add", invalidAdd);

		Clause("clear", c => c(ScheduledCommandProcessor.Clear()));
		
		Clause("remove COMMAND", (command, callback) =>
		{
			callback(ScheduledCommandProcessor.RemoveCommand(command));
		});
		Clause("remove", "Usage: !scheduled remove COMMAND");
	}

	public override ZeroArg Default()
	{
		return c => c("Bad arguments");
	}

	public override string command()
	{
		return "!scheduled";
	}
}
