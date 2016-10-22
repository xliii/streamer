public enum UserRole {

	Streamer,
	Bot,
	Mod,
	Staff,
	Admin,
	GlobalMod,
	Viewer
}

public static class UserRoleExtensions
{
	public static float pointsMultiplier(this UserRole role)
	{
		if (role == UserRole.Viewer) return 1f;

		return 2f;
	}
}
