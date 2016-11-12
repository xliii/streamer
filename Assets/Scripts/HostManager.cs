using System.Text.RegularExpressions;
using UnityEngine;

[RequireComponent(typeof(TwitchIRC))]
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
				int viewers;
				if (!int.TryParse(m.Groups[3].Value, out viewers))
				{
					Debug.LogError("Couldn't parse host viewer amount");
					return;
				}
				Messenger.Broadcast(TwitchAlertsType.host_alert.ToString(), HostAlertData.Create(host, viewers));
			}
		});
	}
}
