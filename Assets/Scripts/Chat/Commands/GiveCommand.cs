using System;
using UnityEngine;

public class GiveCommand : ChatCommand {
	public override void process(User user, string[] args, Action<string> callback)
	{
		if (args.Length < 2)
		{
			callback("USAGE: !give TARGET AMOUNT");
			return;
		}

		var target = UserManager.instance.GetUser(args[0]);
		if (target == null)
		{
			callback("User \"" + args[0] + "\" does not exist");
			return;
		}

		if (user == target)
		{
			callback("You cannot give points to yourself");
			return;
		}

		int amount;
		if (!int.TryParse(args[1], out amount))
		{
			callback("Amount should be an integer");
			return;
		}

		if (amount <= 0)
		{
			callback("Amount should be positive");
			return;
		}

		if (!user.RemovePoints(amount))
		{
			callback("You don't have that much points to give");
			return;
		}
		
		target.AddPoints(amount);
		callback(string.Format("{0} Gave {1} points to {2}", user.username, amount, target.username));
	}

	public override string command()
	{
		return "!give";
	}
}
