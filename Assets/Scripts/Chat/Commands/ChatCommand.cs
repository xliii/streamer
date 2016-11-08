using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ChatCommand : ScriptableObject {

	public List<CommandClause> clauses = new List<CommandClause>();

	//TODO: Replace this with specific error message
	public bool invalid;

	protected ChatCommand()
	{
		Clauses();
		DefaultClause(Default());
	}

	public virtual ZeroArg Default()
	{
		return ctx => ctx.callback("Bad arguments");
	}

	public virtual void Clauses() {}

	public virtual void process(Context context)
	{
		foreach (var clause in clauses)
		{
			if (!clause.Matches(context)) continue;

			if (!clause.AllowedFor(context.user))
			{
				context.callback("You have no rights to do this ;(");
				return;
			}

			clause.Process(context);
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

	private void DefaultClause(ZeroArg response)
	{
		var clause = new CommandClause0(response);
		invalid |= clause.invalid;
		clauses.Add(clause);
	}

	protected CommandClause Clause(string option, string response)
	{
		return Clause(option, ctx => ctx.callback(response));
	}

	protected CommandClause Clause(string option, ZeroArg response)
	{
		var clause = new CommandClause0(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
		return clause;
	}

	protected CommandClause Clause(string option, OneArg response)
	{
		var clause = new CommandClause1(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
		return clause;
	}

	protected CommandClause Clause(string option, TwoArg response)
	{
		var clause = new CommandClause2(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
		return clause;
	}

	public delegate void ZeroArg(Context ctx);
	public delegate void OneArg(string arg1, Context ctx);
	public delegate void TwoArg(string arg1, string arg2, Context ctx);	
}
