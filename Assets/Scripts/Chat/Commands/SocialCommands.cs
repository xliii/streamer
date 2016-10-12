public class TwitchCommand : ChatCommand
{
	public override string command()
	{
		return "!twitch";
	}

	public override string process(string user, string[] args)
	{
		return "http://www.twitch.tv/xliii Kappa";
	}

	public override bool hide()
	{
		return true;	
	}
}

public class TwitterCommand : ChatCommand
{
	public override string command()
	{
		return "!twitter";
	}

	public override string process(string user, string[] args)
	{
		return "http://www.twitter.com/followxliii";
	}
}

public class SoundCloudCommand : ChatCommand
{
	public override string command()
	{
		return "!soundcloud";
	}

	public override string process(string user, string[] args)
	{
		return "http://www.soundcloud.com/followxliii";
	}
}
