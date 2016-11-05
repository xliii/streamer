using System;

public class TwitchCommand : ChatCommand
{
	public override string command()
	{
		return "!twitch";
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("http://www.twitch.tv/xliii Kappa");
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

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("http://www.twitter.com/followxliii");
	}
}

public class SoundCloudCommand : ChatCommand
{
	public override string command()
	{
		return "!soundcloud";
	}

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("http://www.soundcloud.com/followxliii");
	}
}
