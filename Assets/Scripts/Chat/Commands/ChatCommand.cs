using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ChatCommand : ScriptableObject {

	protected List<CommandClause> clauses = new List<CommandClause>();

	protected ChatCommand()
	{
		clauses.Add(Def(Default()));
		Clauses();
	}

	public virtual Func<string> Default()
	{
		return () => "DEFAULT CATCH CLAUSE";
	}

	public virtual void Clauses()
	{
		Debug.Log("Clauses not overriden");
	}

	public virtual void process(User user, string[] args, Action<string> callback)
	{
		foreach (var clause in clauses)
		{
			if (!clause.Matches(args)) continue;

			callback(clause.Process());
			return;
		}
		Debug.LogError("Default clause didn't catch - WE SHOULD NOT BE HERE");
	}

	public abstract string command();

	public virtual bool hide()
	{
		return false;
	}

	public virtual UserRole[] roles()
	{
		return new UserRole[0];
	}

	public virtual int cooldown()
	{
		return 0;
	}

	public static string Normalize(string command)
	{
		return command.StartsWith("!") ? command : "!" + command;
	}

	private CommandClause Def(Func<string> response)
	{
		return new CommandClause0(response);
	}

	protected CommandClause Clause(string option, Func<string> response)
	{
		return new CommandClause0(response, option);
	}

	protected CommandClause Clause(string option, Func<string, string> response)
	{
		return new CommandClause1(response, option);
	}

	protected CommandClause Clause(string option, Func<string, string, string> response)
	{
		return new CommandClause2(response, option);
	}
}
