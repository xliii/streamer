using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
	private string wheel = "Mouse ScrollWheel";
	public float scrollSpeed = 20;

	private Camera cam;
	
	// Use this for initialization
	void Start ()
	{
		cam = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		var scroll = Input.GetAxis(wheel);
		if (scroll != 0)
		{
			if (cam.orthographic)
			{
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll*scrollSpeed, 4, 40);
			}
			else
			{
				cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * scrollSpeed, 10, 80);
			}
		}
	}
}
