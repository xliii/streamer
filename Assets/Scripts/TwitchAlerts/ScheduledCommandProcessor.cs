using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScheduledCommandProcessor
{
	private const string PREFS_KEY = "Scheduled_Commands";
	
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

	public static void Init()
	{
		Commands = Load();
	}

	public static List<ScheduledCommand> Commands = new List<ScheduledCommand>();

	private static void Save()
	{
		PlayerPrefs.SetString(PREFS_KEY,
			Commands.Count == 0 ? "" : Commands.Select(c => c.name + ":" + c.cooldown).Aggregate((cur, next) => cur + ";" + next));
		PlayerPrefs.Save();
	}

	private static List<ScheduledCommand> Load()
	{
		List<ScheduledCommand> list = new List<ScheduledCommand>();
		string prefs = PlayerPrefs.GetString(PREFS_KEY, "");
		if (string.IsNullOrEmpty(prefs))
		{
			return list;
		}

		string[] cmds = prefs.Split(';');
		foreach (var cmd in cmds)
		{
			string[] props = cmd.Split(':');
			if (props.Length != 2)
			{
				Debug.LogError("Bad scheduled commands format: " + prefs);
				return new List<ScheduledCommand>();
			}

			var name = props[0];
			int cooldown;
			if (!int.TryParse(props[1], out cooldown))
			{
				Debug.LogError("Found non-integer cooldown: " + prefs);
				return new List<ScheduledCommand>();
			}

			list.Add(new ScheduledCommand(name, cooldown));
		}
		return list;
	} 

	public static string AddCommand(string command, int cooldown)
	{
		command = ChatCommand.Normalize(command);
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
		Save();
		return result;
	}

	private static bool Contains(string command)
	{
		return Commands.Any(c => c.name == ChatCommand.Normalize(command));
	}

	public static string RemoveCommand(string command)
	{
		int removed = Commands.RemoveAll(c => c.name == ChatCommand.Normalize(command));
		if (removed == 0) return "No such command";
		Save();
		return "Command removed";
	}

	public static string Clear()
	{
		int count = Commands.Count;
		if (count > 0)
		{
			Commands.Clear();
			Save();
			return count + " scheduled commands removed";
		}

		return "No commands scheduled";
	}
}
