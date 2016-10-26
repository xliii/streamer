using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class CommandClause
{
	private string[] option;

	protected List<string> resolvedArgs = new List<string>();

	protected CommandClause(string option = "")
	{
		if (!string.IsNullOrEmpty(option))
		{
			this.option = ParseOption(option);
		}
	}

	private string[] ParseOption(string value)
	{
		return value.Split(' ');
	}

	public bool Matches(string[] args)
	{
		if (option == null)
		{
			//default
			return true;
		}

		if (option.Length != args.Length) return false;

		for (int i = 0; i < args.Length; i++)
		{
			var actual = args[i];
			var formal = option[i];

			//Exact match
			if (formal.ToLower() == formal && formal != actual) return false;
			
			//Placeholder
			if (formal.ToUpper() == formal)
			{
				resolvedArgs.Add(actual);
			}
		}

		return true;
	}

	protected string Arg(int index)
	{
		//TODO: Support keywords: USER, POINTS, COMMAND
		var formal = option[index];
		if (formal == "USER")
		{
			Debug.LogError("CommandClause tried to retrieve USER");
		}
		return resolvedArgs[index];
	}

	public abstract string Process();
}

public class CommandClause0 : CommandClause
{
	private Func<string> response;

	public CommandClause0(Func<string> noArg, string option = "") : base(option)
	{
		response = noArg;
	}

	public override string Process()
	{
		return response();
	}
}

public class CommandClause1 : CommandClause
{
	private Func<string, string> response;

	public CommandClause1(Func<string, string> oneArg, string option = "") : base(option)
	{
		response = oneArg;
	}

	public override string Process()
	{
		return response(Arg(0));
	}
}

public class CommandClause2 : CommandClause
{
	private Func<string, string, string> response;

	public CommandClause2(Func<string, string, string> twoArg, string option = "") : base(option)
	{
		response = twoArg;
	}

	public override string Process()
	{
		return response(Arg(0), Arg(1));
	}
}
