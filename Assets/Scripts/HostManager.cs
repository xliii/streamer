using System.Text.RegularExpressions;
using UnityEngine;

public class HostManager : MonoBehaviour {

	private TwitchIRC irc;

	//:jtv!jtv@jtv.tmi.twitch.tv PRIVMSG xliii :43isgaming is now hosting you for up to 1 viewers.
	private Regex HOST_REGEX = new Regex(@":jtv!jtv@jtv\.tmi\.twitch\.tv PRIVMSG (\w+) :(\w+) is now hosting you for up to (\w+) viewers.");

	// Use this for initialization
	void Start () {
		irc = GetComponent<TwitchIRC>();
		irc.messageRecievedEvent.AddListener(msg =>
		{
			Match m = HOST_REGEX.Match(msg);
			if (m.Success)
			{
				string host = m.Groups[2].Value;
				string viewers = m.Groups[3].Value;
				Debug.Log("Host: " + host + " -> " + viewers);
				//TODO: Actual host alert
			}
		});
	}
}
