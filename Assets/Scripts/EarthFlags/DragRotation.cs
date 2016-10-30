using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotation : MonoBehaviour {

	public float speed = 100;

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			RotateObject();
		}
	}

	void RotateObject()
	{
		transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X"), 0)*Time.deltaTime*speed, Space.Self);
		transform.Rotate(new Vector3(0, 0, Input.GetAxis("Mouse Y")) * Time.deltaTime * speed, Space.World);
	}
}
