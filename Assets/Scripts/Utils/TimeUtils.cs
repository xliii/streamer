using System;

public class TimeUtils {

	private static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

	public static int UnixTimestamp(DateTime date)
	{
		var diff = date - epoch;
		return (int) diff.TotalSeconds;
	}

	public static int UnixTimestamp()
	{
		return UnixTimestamp(DateTime.UtcNow);
	}
}
