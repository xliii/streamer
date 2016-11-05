using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Keyword
{
	public static string POINTS = "{POINTS}";
	public static string USER = "{USER}";

	public static Keyword[] ALL = { new UserKeyword(), new PointsKeyword() };

	public abstract string Name();

	public abstract string Resolve(Context context);
}

public class UserKeyword : Keyword
{
	public override string Name()
	{
		return USER;
	}

	public override string Resolve(Context context)
	{
		return context.user.username;
	}
}

public class PointsKeyword : Keyword
{
	public override string Name()
	{
		return POINTS;
	}

	public override string Resolve(Context context)
	{
		return context.user.Points.ToString();
	}
}