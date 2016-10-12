using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TwitchIRC))]
public class TwitchIRCProcessor : MonoBehaviour {

	TwitchIRC irc;

	public static List<ChatCommand> commands = new List<ChatCommand>();

	void Awake()
	{
		Add(typeof(PingCommand));
		Add(typeof(CommandsCommand));
		Add(typeof(MoreRequestsCommand));
		Add(typeof(TwitchCommand));
		Add(typeof(TwitterCommand));
		Add(typeof(SoundCloudCommand));
		Add(typeof(XliiiCommand));
	}

	private void Add(System.Type t)
	{
		commands.Add(ScriptableObject.CreateInstance(t) as ChatCommand);
	}

	// Use this for initialization
	void Start () {
		irc = GetComponent<TwitchIRC>();
		irc.messageRecievedEvent.AddListener(msg =>
		{
			string[] parts = msg.Split(new char[1] {' '}, 4);
			string user = parts[0].Substring(1, parts[0].IndexOf("!") - 1);
			string type = parts[1];
			string message = parts[3].Substring(1);
			if (type == "PRIVMSG")
			{
				string[] split = message.Split(new char[1] {' '}, 2);
				string command = split[0];
				string[] args;
				if (split.Length > 1)
				{
					args = split[1].Split(' ');
				} else
				{
					args = new string[0];
				}
				foreach (ChatCommand cmd in commands)
				{
					if (command == cmd.command())
					{
						string response = cmd.process(user, args);
						if (!string.IsNullOrEmpty(response))
						{
							irc.SendMsg(response);
						}
					}
				}
			} else
			{
				Debug.LogWarning("Unknown event received: " + msg);
			}
		});
	}
}
