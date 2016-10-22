using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(TwitchIRC))]
[RequireComponent(typeof(TwitchAPI))]
public class TwitchIRCProcessor : MonoBehaviour {

	TwitchIRC irc;

	public static List<ChatCommand> commands = new List<ChatCommand>();	

	private Dictionary<string, float> lastUsages = new Dictionary<string, float>();

	private bool onCooldown(ChatCommand command)
	{
		if (!lastUsages.ContainsKey(command.command())) return false;

		return Time.time - lastUsages[command.command()] < command.cooldown();
	}

	void Awake()
	{
		RegisterChatCommands();
	}

	void RegisterChatCommands()
	{
		List<Type> types = typeof (ChatCommand).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof (ChatCommand))).ToList();
		foreach (var type in types)
		{
			Add(type);
		}
	}

	void Update()
	{
		//Process scheduled messages
		foreach (var scheduled in ScheduledCommandProcessor.Commands)
		{
			if (scheduled.lastUsage == 0)
			{
				scheduled.lastUsage = Time.time;
				continue;
			}

			if (scheduled.lastUsage + scheduled.cooldown > Time.time) continue;

			scheduled.lastUsage = Time.time;
			ChatCommand cmd = Get(scheduled.name);
			if (cmd == null)
			{
				Debug.LogError("Scheduled command was null - THIS SHOULD NOT HAPPEN");
				return;
			}
			Process(cmd, UserManager.instance.GetBot(), new string[0]);
		}
	}

	private static ChatCommand Get(string commandName)
	{
		return commands.Find(command => command.command() == commandName);
	}

	public static bool HasCommand(string commandName)
	{
		return commands.Any(command => command.command() == commandName);
	}

	private void Add(Type t)
	{
		commands.Add(ScriptableObject.CreateInstance(t) as ChatCommand);
	}

	private void Process(ChatCommand command, User user, string[] args)
	{
		command.process(user, args, response =>
		{
			if (!string.IsNullOrEmpty(response))
			{
				irc.SendMsg(response);
			}
		});
	}

	// Use this for initialization
	void Start () {
		ScheduledCommandProcessor.Init();
		irc = GetComponent<TwitchIRC>();
		irc.messageRecievedEvent.AddListener(msg =>
		{
			string[] parts = msg.Split(new char[1] {' '}, 4);
			string username = parts[0].Substring(1, parts[0].IndexOf("!") - 1);
			string type = parts[1];
			string message = parts[3].Substring(1);
			if (type == "PRIVMSG")
			{
				string[] split = message.Split(new char[1] {' '}, 2);
				string command = split[0];
				var args = split.Length > 1 ? split[1].Split(' ') : new string[0];

				ChatCommand cmd = Get(command);

				if (cmd == null) return;

				if (onCooldown(cmd)) return;

				User user = UserRepository.GetByUsername(username);

				if (user == null)
				{
					Debug.LogError(string.Format("Could not retrieve user {0} - THIS SHOULD NOT HAPPEN", username));
					return;
				}

				if (!user.HasAnyRole(cmd.roles()))
				{
					irc.SendMsg("You have no rights to execute this command");
					return;
				}
				
				lastUsages[command] = Time.time;
				Process(cmd, user, args);
				//Return after processing a single command
				return;
			}
			Debug.LogWarning("Unknown event received: " + msg);
		});
	}
}
