public class PointsCommand : ChatCommand {

	public override ZeroArg Default()
	{
		return c => c("Usage: !points");
	}

	public override void Clauses()
	{
		//TODO: Resolve caller's points from context
		Clause("", c => c("That's a secret Kappa"));
		Clause("USERNAME", (username, callback) =>
		{
			var target = UserRepository.GetByUsername(username);

			if (target == null)
			{
				callback("User \"" + username + "\" does not exist");
				return;
			}
			
			//TODO: Check for oneself - we need context

			callback(target.username + " has " + target.Points + " points");
		});
	}

	public override string command()
	{
		return "!points";
	}
}
