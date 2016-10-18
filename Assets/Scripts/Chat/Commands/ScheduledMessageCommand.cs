using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScheduledMessageCommand : ChatCommand
{

	private const int MIN_COOLDOWN = 1;

	public override void process(string user, string[] args, Action<string> callback)
	{
		if (args.Length < 1)
		{
			callback("Wrong arguments :(");
			return;
		}

		if (args[0] == "list")
		{
			if (ScheduledCommandProcessor.Commands.Count == 0)
			{
				callback("No commands scheduled");
				return;
			}
			//TODO: Include cooldowns
			string commands = ScheduledCommandProcessor.Commands.Select(c => c.name).Aggregate((cur, next) => cur + ", " + next);
			callback("Scheduled commands: " + commands);
			return;
		}

		if (args[0] == "add")
		{
			if (args.Length < 3)
			{
				callback("Usage: !scheduled add COMMAND COOLDOWN");
				return;
			}

			int cooldown;
			if (!int.TryParse(args[2], out cooldown))
			{
				callback("Cooldown should be integer");
				return;
			}

			if (cooldown < MIN_COOLDOWN)
			{
				callback("Cooldown should be at least " + MIN_COOLDOWN);
				return;
			}

			ScheduledCommandProcessor.AddCommand(args[1], cooldown);
			callback("Scheduled command added");
			return;
		}


		if (args[0] == "remove")
		{
			//remove command
			callback("!scheduled remove");
			return;
		}
	}

	public override string command()
	{
		return "!scheduled";
	}

	public override int cooldown()
	{
		return 0;
	}
}
