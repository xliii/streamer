using System.Collections.Generic;
using UnityEngine;

public class AlertCommand : ChatCommand
{
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
			FakeHost();
			ctx.callback("Test host triggered");
		});
	}

	private List<string> messages = new List<string>()
	{
		"Nice stream bro Kappa"
	};

	private void FakeHost()
	{
		Messenger.Broadcast(TwitchAlertsType.host_alert.ToString(), HostAlertData.Create(TestUser(), Random.Range(10, 100)));
	}

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
		int money = 2 * Random.Range(1, 70);
		var donation = DonationAlertData.Create(TestUser(), money, RandomDonationMessage());
		Messenger.Broadcast(TwitchAlertsType.most_recent_donator.ToString(), donation);
	}

	private void FakeFollow()
	{
		//TODO: inc total follows
		Messenger.Broadcast(TwitchAlertsType.most_recent_follower.ToString(), FollowAlertData.Create(TestUser()), false);
	}

	private string TestUser()
	{
		return "TestUser" + Random.Range(1, 999);
	}
}
