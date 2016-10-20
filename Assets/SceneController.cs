using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour
{
	public const string BASE = "Base";
	public const string STARTING_SOON = "StartingSoon";
	public const string CODING = "Coding";
	public const string FULLSCREEN = "FullScreen";
	public const string MUSIC = "Music";
	public const string SANDBOX = "Sandbox";

	public static string sceneToLoad = STARTING_SOON;

	private static Dictionary<string, KeyCode> scenes = new Dictionary<string, KeyCode>
	{
		{ STARTING_SOON, KeyCode.F1 },
		{ CODING, KeyCode.F2 },
		{ FULLSCREEN, KeyCode.F3 },
		{ MUSIC, KeyCode.F4 },
		{ BASE, KeyCode.None },
		{ SANDBOX, KeyCode.None }
	};

	public static List<string> AllScenes()
	{
		return scenes.Keys.ToList();
	}
	
	void Start () {
		if (sceneToLoad != LoadedScene())
		{
			Load(sceneToLoad);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		foreach (var entry in scenes)
		{
			if (entry.Value == KeyCode.None) continue;

			if (Input.GetKeyDown(entry.Value))
			{
				Load(entry.Key);
				return;
			}
		}
	}
	
	void Load(string scene)
	{
		foreach (var s in scenes)
		{
			if (s.Value == KeyCode.None) continue;

			if (!SceneManager.GetSceneByName(s.Key).isLoaded) continue;

			SceneManager.UnloadSceneAsync(s.Key);
		}
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}

	public static bool IsLoaded(string scene)
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).name == scene) return true;			
		}
		return false;
	}

	public static string LoadedScene()
	{
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			Scene scene = SceneManager.GetSceneAt(i);
			if (scene.name == BASE) continue;

			return scene.name;
		}
		return "";
	}
}
