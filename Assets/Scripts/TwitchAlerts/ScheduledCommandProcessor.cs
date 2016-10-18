using System.Collections;
using System.Collections.Generic;
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

	public static void AddCommand(string command, int cooldown)
	{
		//TODO: Deal with duplicates (ignore or override?)
		if (!command.StartsWith("!"))
		{
			command = "!" + command;
		}

		if (!TwitchIRCProcessor.HasCommand(command))
		{
			Debug.LogWarning("No such command: " + command);
			return;
		}

		Commands.Add(new ScheduledCommand(command, cooldown));
	}

	public static void RemoveCommand(string command)
	{
		
	}
}
