using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
			return;
		}

		if (Input.GetKeyDown(KeyCode.F1))
		{
			Load("StartingSoon");
			return;
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			Load("Main");
			return;
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			Load("Notifications");
			return;
		}
	}

	void Load(string scene)
	{
		SceneManager.LoadScene(scene);
		AlertManager.alertInProgress = false;
	}	
}
