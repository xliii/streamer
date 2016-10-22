using System;

public class PointsCommand : ChatCommand {
	public override void process(User user, string[] args, Action<string> callback)
	{
		if (args.Length == 0)
		{
			callback("You have " + user.points + " points");
			return;
		}

		var target = UserManager.instance.GetUser(args[0]);

		if (target == null)
		{
			callback("User \"" + args[0] + "\" does not exist");
			return;
		}

		callback(target.username + " has " + target.points + " points");
	}

	public override int cooldown()
	{
		return 0;
	}

	public override string command()
	{
		return "!points";
	}
}
