using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class CommandClause
{
	private string[] option;

	protected List<string> resolvedArgs = new List<string>();

	public bool invalid;

	protected CommandClause(string option = "")
	{
		if (!string.IsNullOrEmpty(option))
		{
			this.option = ParseOption(option);
		}
	}

	private string[] ParseOption(string value)
	{
		string[] args = value.Split(' ');
		//Formal arguments validation
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == ChatCommand.REST && i != args.Length - 1)
			{
				//REST should be the last formal argument
				invalid = true;
				break;
			}
		}
		return args;
	}

	public bool Matches(string[] args)
	{
		if (option == null)
		{
			//default
			return true;
		}

		if (option.Length > args.Length) return false;

		for (int i = 0; i < option.Length; i++)
		{
			var actual = args[i];
			var formal = option[i];

			if (formal == ChatCommand.REST)
			{
				//Rest of the actual arguments as one
				resolvedArgs.Add(string.Join(" ", args.Skip(i).ToArray()));
				break;
			}

			//Exact match
			if (formal.ToLower() == formal && formal != actual) return false;

			resolvedArgs.Add(actual);
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

	public abstract void Process(Action<string> callback);
}

public class CommandClause0 : CommandClause
{
	private ChatCommand.ZeroArg response;

	public CommandClause0(ChatCommand.ZeroArg zeroArg, string option = "") : base(option)
	{
		response = zeroArg;
	}

	public override void Process(Action<string> callback)
	{
		response(callback);
	}
}

public class CommandClause1 : CommandClause
{
	private ChatCommand.OneArg response;

	public CommandClause1(ChatCommand.OneArg oneArg, string option = "") : base(option)
	{
		response = oneArg;
	}

	public override void Process(Action<string> callback)
	{
		response(Arg(0), callback);
	}
}

public class CommandClause2 : CommandClause
{
	private ChatCommand.TwoArg response;

	public CommandClause2(ChatCommand.TwoArg twoArg, string option = "") : base(option)
	{
		response = twoArg;
	}

	public override void Process(Action<string> callback)
	{
		response(Arg(0), Arg(1), callback);
	}
}
