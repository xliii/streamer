using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TwitchEmotes
{
	private static Dictionary<string, Vector2> emotes;

	private static void InitEmotes()
	{
		emotes = new Dictionary<string, Vector2>
		{
			{"WUTFACE", new Vector2(5,1)},
			{"WINWAKER", new Vector2(4,1)},
			{"VAULTBOY", new Vector2(5,2)},
			{"TWITCHRAID", new Vector2(3,0)},
			{"TTOURS", new Vector2(3,1)},
			{"TRIHARD", new Vector2(4,2)},
			{"SWIFTRAGE", new Vector2(2,0)},
			{"SSSSSS", new Vector2(3,2)},
			{"SMORC", new Vector2(2,1)},
			{"SHIBEZ", new Vector2(1,0)},
			{"SEEMSGOOD", new Vector2(1,1)},
			{"RIPEPPERONIS", new Vector2(0,0)},
			{"RESIDENTSLEEPER", new Vector2(2,2)},
			{"RACCATTACK", new Vector2(1,2)},
			{"PUPPEYFACE", new Vector2(0,1)},
			{"PUNCHTREES", new Vector2(0,2)},
			{"POGCHAMP", new Vector2(6,3)},
			{"PJSALT", new Vector2(5,3)},
			{"OSSLOTH", new Vector2(6,4)},
			{"OSKOMODO", new Vector2(6,5)},
			{"KAPPA", new Vector2(2,4)},
			{"DEILLUMINATI", new Vector2(2,5)},
			{"KEEPO", new Vector2(4,6)},
			{"OPIEOP", new Vector2(5,4)},
			{"KAPPAHD", new Vector2(2,3)},
			{"4HEAD", new Vector2(0,6)},
			{"MINIK", new Vector2(6,6)},
			{"KREYGASM", new Vector2(5,6)},
			{"IMGLITCH", new Vector2(1,4)},
			{"OSFROG", new Vector2(4,3)},
			{"DENDIFACE", new Vector2(3,6)},
			{"FRANKERZ", new Vector2(2,6)},
			{"HEYGUYS", new Vector2(0,3)},
			{"BIBLETHUMP", new Vector2(0,5)},
			{"NOTLIKETHIS", new Vector2(4,4)},
			{"MINGLEE", new Vector2(4,5)},
			{"KAPPAPRIDE", new Vector2(3,4)},
			{"KAPPACLAUS", new Vector2(1,3)},
			{"COOLCAT", new Vector2(1,5)},
			{"CORGIDERP", new Vector2(2,6)},
			{"DUDUDU", new Vector2(3,5)},
			{"MVGAME", new Vector2(5,5)},
			{"KAPPAROSS", new Vector2(3,3)},
			{"BABYRAGE", new Vector2(1,6)},
		};
	}

	private static Dictionary<string, Vector2> Emotes()
	{
		if (emotes == null)
		{
			InitEmotes();
		}
		return emotes;
	}

	public static string RandomEmote()
	{
		return Emotes().Keys.ToArray()[Random.Range(0, Emotes().Keys.Count - 1)];
	}

	public static bool IsEmote(string word)
	{
		return Emotes().ContainsKey(word.ToUpper());
	}

	public static Vector2 GetPos(string word)
	{
		return Emotes()[word.ToUpper()];
	}
}
