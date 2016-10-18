using UnityEngine;

public class TestOBS : MonoBehaviour
{

	public KeyCode leftClick;
	public KeyCode rightClick;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			OBSRemote.SendKey(leftClick);
		} else if (Input.GetMouseButtonDown(1))
		{
			OBSRemote.SendKey(rightClick);
		}
	}
}
