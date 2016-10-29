using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour {

	public GameObject square;
	public GameObject sphere;

	public float radius = 20;

	public Transform map;

	public float latitude;
	public float longitude;

	public string place;

	Vector3 pos;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			TwitchAPI.Geolocate(place, (formatted, lt, ln) =>
			{
				latitude = lt;
				longitude = ln;
			});
		}
		latitude = Normalize(latitude, 90);
		longitude = Normalize(longitude, 180);
		SetSquare();
		SetSphere();
	}

	void SetSquare()
	{
		if (square == null || map == null) return;
		float offsetX = map.position.z - map.localScale.x*5;
		float offsetY = map.position.y - map.localScale.y*5;
		square.transform.position = new Vector3(0, offsetY + Y * map.localScale.y * 10, offsetX + X * map.localScale.x * 10);
	}

	void SetSphere()
	{
		var u = X + 0.5f;
		if (u > 1)
		{
			u -= 1;
		}
		var atan = (u - 1) * 2 * Mathf.PI;
		var asin = (Y - 0.5f) * Mathf.PI;
		var y = Mathf.Sin(asin);
		var length = Mathf.Sqrt(1 - y * y);
		var x = Mathf.Cos(atan) * length; //TODO:
		var z = Mathf.Sin(atan) * length; //Fix these two. Use length in someway
		
		//var u - 1f= Mathf.Atan2(one.z, one.x) / (2 * Mathf.PI);
		//var v - 0.5f = Mathf.Asin(one.y) / Mathf.PI;
		sphere.transform.localPosition = new Vector3(x, y, z) * 20;
	}

	float X
	{
		get { return (longitude + 180) / 360; }
	}

	float Y
	{
		get { return (latitude + 90) / 180; }
	}

	float Normalize(float value, float limit)
	{
		if (value > limit) return -limit;
		if (value < -limit) return limit;
		return value;
	}
}
