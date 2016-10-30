using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
	private string wheel = "Mouse ScrollWheel";
	public float scrollSpeed = 20;

	private Camera camera;
	
	// Use this for initialization
	void Start ()
	{
		camera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		var scroll = Input.GetAxis(wheel);
		if (scroll != 0)
		{
			if (camera.orthographic)
			{
				camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scroll*scrollSpeed, 4, 40);
			}
			else
			{
				camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - scroll * scrollSpeed, 10, 80);
			}
		}
	}
}
