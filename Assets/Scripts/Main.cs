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
			SceneManager.LoadScene("StartingSoon");
			return;
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			SceneManager.LoadScene("Main");
			return;
		}

		if (Input.GetKeyDown(KeyCode.F3))
		{
			SceneManager.LoadScene("Notifications");
			return;
		}

	}
}
