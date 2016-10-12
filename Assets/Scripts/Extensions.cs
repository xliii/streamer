using UnityEngine;

public static class Extensions
{
	public static bool IsDeepBot(this TwitchAlertsType type)
	{
		switch (type)
		{
			case TwitchAlertsType.YoutubeCurrentSong:
			case TwitchAlertsType.YoutubeCurrentSongRequestedBy:
				return true;
			default:
				return false;
		}
	}

	public static AnimationProcessor AddProcessor(this AnimationType type, GameObject gameObject)
	{
		switch (type)
		{
			case AnimationType.FlipHorizontal:
				return gameObject.AddComponent<AnimationFlipHorizontal>();
			case AnimationType.FlipVertical:
				return gameObject.AddComponent<AnimationFlipVertical>();
			default:
				return gameObject.AddComponent<AnimationNone>();
		}
	}
}