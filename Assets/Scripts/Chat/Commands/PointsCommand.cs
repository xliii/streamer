using System;

public class PointsCommand : ChatCommand {
	public override void process(User user, string[] args, Action<string> callback)
	{
		if (args.Length == 0)
		{
			callback("You have " + user.Points + " points");
			return;
		}

		var target = UserManager.instance.GetUser(args[0]);

		if (target == null)
		{
			callback("User \"" + args[0] + "\" does not exist");
			return;
		}

		callback(target.username + " has " + target.Points + " points");
	}

	public override string command()
	{
		return "!points";
	}
}
