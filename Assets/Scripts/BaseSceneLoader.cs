using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneLoader : MonoBehaviour {
	
	void Start () {
		if (!SceneController.IsLoaded(SceneController.BASE))
		{
			string loaded = SceneController.LoadedScene();
			if (string.IsNullOrEmpty(loaded))
			{
				Debug.LogError("Could not determine loaded scene :(");
				return;
			}

			SceneController.sceneToLoad = loaded;
			SceneManager.LoadScene(SceneController.BASE);
		}
	}

}
