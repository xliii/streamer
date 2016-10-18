using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScheduledCommandProcessor {

	//TODO: Store scheduled commands between sessions
	public class ScheduledCommand
	{
		public string name;
		public int cooldown;
		public float lastUsage;

		public ScheduledCommand(string name, int cooldown)
		{
			this.name = name;
			this.cooldown = cooldown;
		}

		public override string ToString()
		{
			TimeSpan time = TimeSpan.FromSeconds(cooldown);
			if (time.Hours > 0)
			{
				return string.Format("{0} ({1}:{2}:{3})", name, time.Hours, time.Minutes, time.Seconds);
			}
			else
			{
				return string.Format("{0} ({1}:{2})", name, time.Minutes, time.Seconds);
			}
		}
	}

	public static List<ScheduledCommand> Commands = new List<ScheduledCommand>();

	public static string AddCommand(string command, int cooldown)
	{
		command = Fix(command);
		if (!TwitchIRCProcessor.HasCommand(command))
		{
			return "No such command";
		}

		string result;
		if (Contains(command))
		{
			RemoveCommand(command);
			result = "Command replaced";
		}
		else
		{
			result = "Command added";
		}
		Commands.Add(new ScheduledCommand(command, cooldown));
		return result;
	}

	private static bool Contains(string command)
	{
		return Commands.Any(c => c.name == Fix(command));
	}

	public static string RemoveCommand(string command)
	{
		int removed = Commands.RemoveAll(c => c.name == Fix(command));
		return removed == 0 ? "No such command" : "Command removed";
	}

	private static string Fix(string command)
	{
		return command.StartsWith("!") ? command : "!" + command;
	}
}
