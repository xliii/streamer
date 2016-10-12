using UnityEngine;

public class TestOBS : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			OBSRemote.SendKey(KeyCode.F13);
		} else if (Input.GetMouseButtonDown(1))
		{
			OBSRemote.SendKey(KeyCode.F14);
		}
	}
}
