using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScheduledCommandProcessor {

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
	}

	public static List<ScheduledCommand> Commands = new List<ScheduledCommand>();

	public static string AddCommand(string command, int cooldown)
	{
		//TODO: Override duplicates
		command = Fix(command);
		if (!TwitchIRCProcessor.HasCommand(command))
		{
			return "No such command";
		}

		var result = Contains(command) ? "Command replaced" : "Command added";
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
