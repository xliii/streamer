using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertCommand : ChatCommand {
	public override string command()
	{
		return "!alert";
	}

	public override UserRole[] roles()
	{
		return new UserRole[] {UserRole.Streamer};
	}

	public override void Clauses()
	{
		Clause("test donation", ctx =>
		{
			FakeDonation();
			ctx.callback("Test donation triggered");
		});
		Clause("test follow", ctx =>
		{
			FakeFollow();
			ctx.callback("Test follow triggered");
		});
		Clause("test host", ctx =>
		{
			ctx.callback("Test host triggered");
			Debug.Log("Test host!");
		});
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

	private void FakeDonation()
	{
		string testUser = "TestUser" + Random.Range(1, 999);
		int money = 2 * Random.Range(1, 70);
		string amount = "($" + money + ".00)";
		string message = testUser + " " + amount + TextFromFile.DELIMETER + RandomDonationMessage();
		TextFromFile.WriteOnce(TwitchAlertsType.most_recent_donator, message);
	}

	private void FakeFollow()
	{
		string testUser = "TestUser" + Random.Range(1, 999);
		int followers = int.Parse(TextFromFile.ReadOnce(TwitchAlertsType.session_follower_count));
		TextFromFile.WriteOnce(TwitchAlertsType.most_recent_follower, testUser);
		TextFromFile.WriteOnce(TwitchAlertsType.session_follower_count, (followers + 1).ToString());
	}
}
