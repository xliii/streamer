using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class ChatCommand : ScriptableObject {

	protected List<CommandClause> clauses = new List<CommandClause>();

	//TODO: Replace this with specific error message
	public bool invalid;

	protected ChatCommand()
	{
		Clauses();
		DefaultClause(Default());
	}

	public virtual ZeroArg Default()
	{
		Debug.Log("Default not implemented for " + command());
		return callback => callback("DEFAULT CATCH CLAUSE");
	}

	public virtual void Clauses() {}

	public virtual void process(Context context)
	{
		foreach (var clause in clauses)
		{
			if (!clause.Matches(context)) continue;

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

	protected void Clause(string option, string response)
	{
		Clause(option, c => c(response));
	}

	protected void Clause(string option, ZeroArg response)
	{
		var clause = new CommandClause0(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
	}

	protected void Clause(string option, OneArg response)
	{
		var clause = new CommandClause1(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
	}

	protected void Clause(string option, TwoArg response)
	{
		var clause = new CommandClause2(response, option);
		invalid |= clause.invalid;
		clauses.Add(clause);
	}

	public delegate void ZeroArg(Action<string> callback);
	public delegate void OneArg(string arg1, Action<string> callback);
	public delegate void TwoArg(string arg1, string arg2, Action<string> callback);
}
