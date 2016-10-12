using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TwitchAlertsTest : MonoBehaviour
{
	public bool donationMode;

	public KeyCode key = KeyCode.F10;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(key))
		{
			if (donationMode)
			{
				FakeDonation();
			}
			else
			{
				FakeFollow();
			}
		}
	}

	private void FakeDonation()
	{
		string testUser = "TestUser" + Random.Range(1, 999);
		int money = 2 * Random.Range(1, 70);
		string amount = "($" + money + ".00)";
		string message = testUser + " " + amount + TextFromFile.DELIMETER + RandomDonationMessage();
		TextFromFile.WriteOnce(TwitchAlertsType.most_recent_donator, message);
	}

	private List<string> messages = new List<string>()
	{
		"Nice stream bro Kappa"
	};

	private string RandomDonationMessage(int emotes = 0)
	{
		string message = messages[Random.Range(0, messages.Count - 1)];
		for (int i = 0; i < emotes; i++)
		{
			message += TwitchEmotes.RandomEmote() + " ";
		}
		return message;
	}

	private void FakeFollow()
	{
		string testUser = "TestUser" + Random.Range(1, 999);
		int followers = int.Parse(TextFromFile.ReadOnce(TwitchAlertsType.session_follower_count));
		TextFromFile.WriteOnce(TwitchAlertsType.most_recent_follower, testUser);
		TextFromFile.WriteOnce(TwitchAlertsType.session_follower_count, (followers + 1).ToString());
	}
}
