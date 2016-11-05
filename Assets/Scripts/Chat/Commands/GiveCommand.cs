public class GiveCommand : ChatCommand {

	public override void Clauses()
	{
		Clause("TARGET AMOUNT", (target, amountString, ctx) =>
		{
			var targetUser = UserRepository.GetByUsername(target);
			if (targetUser == null)
			{
				ctx.callback("User \"" + target + "\" does not exist");
				return;
			}

			if (ctx.user == targetUser)
			{
				ctx.callback("You cannot give points to yourself");
				return;
			}

			int amount;
			if (!int.TryParse(amountString, out amount))
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

			targetUser.AddPoints(amount);
			UserRepository.Save(ctx.user);
			UserRepository.Save(targetUser);
			ctx.callback(string.Format("{0} Gave {1} points to {2}", ctx.user.username, amount, targetUser.username));
		});
	}

	public override string command()
	{
		return "!give";
	}
}
