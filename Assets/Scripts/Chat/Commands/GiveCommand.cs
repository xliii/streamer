public class GiveCommand : ChatCommand {
	public override void process(Context ctx)
	{
		if (ctx.args.Length < 2)
		{
			ctx.callback("USAGE: !give TARGET AMOUNT");
			return;
		}

		var target = UserRepository.GetByUsername(ctx.args[0]);
		if (target == null)
		{
			ctx.callback("User \"" + ctx.args[0] + "\" does not exist");
			return;
		}

		if (ctx.user == target)
		{
			ctx.callback("You cannot give points to yourself");
			return;
		}

		int amount;
		if (!int.TryParse(ctx.args[1], out amount))
		{
			ctx.callback("Amount should be an integer");
			return;
		}

		if (amount <= 0)
		{
			ctx.callback("Amount should be positive");
			return;
		}

		if (!ctx.user.RemovePoints(amount))
		{
			ctx.callback("You don't have that much points to give");
			return;
		}
		
		target.AddPoints(amount);
		UserRepository.Save(ctx.user);
		UserRepository.Save(target);
		ctx.callback(string.Format("{0} Gave {1} points to {2}", ctx.user.username, amount, target.username));
	}

	public override string command()
	{
		return "!give";
	}
}
