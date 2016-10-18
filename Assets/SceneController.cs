using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour {

	private static Dictionary<string, KeyCode> scenes = new Dictionary<string, KeyCode>
	{
		{ "StartingSoon", KeyCode.F1 },
		{ "Coding", KeyCode.F2 },
		{ "FullScreen", KeyCode.F3 },
		{ "Music", KeyCode.F4 },
		{ "Base", KeyCode.None },
		{ "Sandbox", KeyCode.None }
	};

	public static List<string> AllScenes()
	{
		return scenes.Keys.ToList();
	} 
	
	void Start () {
		Load("StartingSoon");
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
	
	void Load (string scene)
	{
		foreach (var s in scenes)
		{
			if (SceneManager.GetSceneByName(s.Key).isLoaded)
			{
				SceneManager.UnloadSceneAsync(s.Key);
			}
		}
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}	
}
