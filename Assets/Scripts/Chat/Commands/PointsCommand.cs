public class PointsCommand : ChatCommand {

	public override ZeroArg Default()
	{
		return ctx => ctx.callback("Usage: !points");
	}

	public override void Clauses()
	{
		//TODO: Resolve caller's points from context
		Clause("", ctx => ctx.callback("You have " + Keyword.POINTS + " points"));
		Clause(Keyword.USER, (ctx => ctx.callback("You have " + Keyword.POINTS + " points")));
		Clause("USERNAME", (username, ctx) =>
		{
			var target = UserRepository.GetByUsername(username);

			if (target == null)
			{
				ctx.callback("User \"" + username + "\" does not exist");
				return;
			}

			ctx.callback(target.username + " has " + target.Points + " points");
		}).Roles(UserRole.Streamer, UserRole.Mod);
	}

	public override string command()
	{
		return "!points";
	}
}
