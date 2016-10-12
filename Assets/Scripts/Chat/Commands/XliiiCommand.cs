using System;

public class XliiiCommand : ChatCommand
{
	private Random random = new Random();

	private string[] responses =
	{
		"I've chosen this nickname because that's my shoe size, easy as that CorgiDerp",
		"I've chosen this nickname because Technetium is my favorite chemical element CoolCat",
		"I've chosen this nickname because my favorite CS weapon was M4A1 with a shortcut B -> 4 -> 3 4Head",
		"I've chosen this nickname because my favorite baseball pitcher Dennis \"Eck\" Eckersley's uniform number was 43 FrankerZ",
		"I've chosen this nickname because Movie 43 is my favorite movie OpieOP",
		"I've chosen this nickname because I used to call my girlfriend who lived in Austria, which country code is +43 SMOrc"
	};

	public override string command()
	{
		return "!xliii";
	}

	public override string process(string user, string[] args)
	{
		return responses[random.Next(responses.Length)];
	}
}
