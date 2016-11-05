using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class CommandClause
{
	private string[] option;
	private string description = "";

	public bool invalid;

	//TODO: role support as with description
	public CommandClause Description(string description)
	{
		this.description = description;
		return this;
	}

	public string Description()
	{
		return description;
	}

	public string Option()
	{
		if (option == null)
		{
			return "";
		}
		return string.Join(" ", option);
	}

	protected CommandClause(string option = null)
	{
		if (option != null)
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
			if (args[i] == Param.REST && i != args.Length - 1)
			{
				//REST should be the last formal argument
				invalid = true;
				break;
			}
		}
		return args;
	}

	public bool Matches(Context context)
	{
		if (option == null)
		{
			//default
			return true;
		}

		//Check for 0 args
		if (option.Length == 1 && option[0] == "" && context.args.Length == 0) return true;

		if (option.Length > context.args.Length) return false;

		for (int i = 0; i < option.Length; i++)
		{
			var actual = context.args[i];
			var formal = option[i];

			if (formal == Param.REST)
			{
				//Rest of the actual arguments as one
				context.resolvedArgs.Add(string.Join(" ", context.args.Skip(i).ToArray()));
				break;
			}

			//Command option
			if (formal.ToLower() == formal)
			{
				if (formal != actual) return false;
				//We don't want to add this one to resolved args
				continue;
			}

			bool resolved = false;
			foreach (Keyword keyword in Keyword.ALL)
			{
				if (keyword.Name() != formal) continue;

				var expected = keyword.Resolve(context);
				if (expected != actual) return false;

				context.resolvedArgs.Add(expected);
				resolved = true;
				break;
			}

			if (resolved) continue;

			context.resolvedArgs.Add(actual);
		}

		return true;
	}

	//This should be ALWAYS called, since we do the callback switching
	public virtual void Process(Context context)
	{
		Action<string> callback = context.callback;
		context.callback = s => callback(ResolveKeywords(s, context));
	}

	protected string ResolveKeywords(string response, Context context)
	{		
		foreach (var keyword in Keyword.ALL)
		{
			response = response.Replace(keyword.Name(), keyword.Resolve(context));			
		}
		return response;
	}
}

public class CommandClause0 : CommandClause
{
	private ChatCommand.ZeroArg response;

	public CommandClause0(ChatCommand.ZeroArg zeroArg, string option = null) : base(option)
	{
		response = zeroArg;
	}

	public override void Process(Context context)
	{
		base.Process(context);
		response(context);
	}
}

public class CommandClause1 : CommandClause
{
	private ChatCommand.OneArg response;

	public CommandClause1(ChatCommand.OneArg oneArg, string option = null) : base(option)
	{
		response = oneArg;
	}

	public override void Process(Context context)
	{
		base.Process(context);
		response(context[0], context);
	}
}

public class CommandClause2 : CommandClause
{
	private ChatCommand.TwoArg response;

	public CommandClause2(ChatCommand.TwoArg twoArg, string option = null) : base(option)
	{
		response = twoArg;
	}

	public override void Process(Context context)
	{
		base.Process(context);
		response(context[0], context[1], context);
	}
}

public class Context
{
	public User user;
	public string[] args;
	public Action<string> callback;
	public List<string> resolvedArgs = new List<string>();

	public Context(User user, string[] args, Action<string> callback)
	{
		this.user = user;
		this.args = args;
		this.callback = callback;
	}

	public string this[int i]
	{
		get { return resolvedArgs[i]; }
	}
}
