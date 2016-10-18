using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	private Dictionary<KeyCode, string> scenes = new Dictionary<KeyCode, string>
	{
		{ KeyCode.F1, "StartingSoon" },
		{ KeyCode.F2, "Coding" },
		{ KeyCode.F3, "FullScreen" },
		{ KeyCode.F4, "Music" },
	};
	
	void Start () {
		Load(scenes[KeyCode.F1]);
	}

	void Update()
	{
		foreach (var entry in scenes)
		{
			if (Input.GetKeyDown(entry.Key))
			{
				Load(entry.Value);
				return;
			}
		}
	}
	
	void Load(string scene)
	{
		foreach (var s in scenes)
		{
			if (SceneManager.GetSceneByName(s.Value).isLoaded)
			{
				SceneManager.UnloadSceneAsync(s.Value);
			}
		}
		SceneManager.LoadScene(scene, LoadSceneMode.Additive);
	}	
}
