using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutFlag : MonoBehaviour
{
	public GameObject flagPrefab;

	public float latitude;
	public float longitude;
	public float distance = 20;

	public Camera front;
	public Camera back;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
		{
			//add flag
			Camera cam = Input.GetMouseButtonDown(0) ? front : back;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Quaternion rotation = Quaternion.identity;
				rotation = Quaternion.LookRotation(hit.normal);
				GameObject flag = Instantiate(flagPrefab, hit.point, rotation);
				flag.transform.SetParent(transform);
				var one = hit.point/distance;
				//Debug.Log(one);
				var u = 1f + Mathf.Atan2(one.z, one.x)/(2*Mathf.PI);
				if (u > 1)
				{
					u -= 1;
				}
				var v = 0.5f + Mathf.Asin(one.y)/Mathf.PI;
				//Debug.Log(u + " : " + v);
			}
		}
		/*if (Input.GetMouseButtonDown(1))
		{
			var pos = Quaternion.AngleAxis(longitude, -Vector3.up) * Quaternion.AngleAxis(latitude, -Vector3.right) * new Vector3(0, 0, distance);
			var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.SetParent(transform);
			cube.transform.position = pos;
		}*/
	}
}
