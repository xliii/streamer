using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutFlag : MonoBehaviour
{
	public GameObject flagPrefab;

	private Dictionary<Flag, Transform> flags = new Dictionary<Flag, Transform>();

	void Start()
	{
		Messenger.AddListener<Flag>(Flag.FLAG_ADDED, Put);
		Messenger.AddListener(Flag.FLAGS_CLEARED, Clear);
		PutFlags();
	}

	void Clear()
	{
		foreach (Transform t in flags.Values)
		{
			//TODO: Add visuals
			Destroy(t.gameObject);
		}
	}

	void PutFlags()
	{
		var all = FlagRepository.GetAll();
		Debug.Log("Putting " + all.Count + " flags");
		foreach (Flag flag in all)
		{
			Put(flag);
		}
	}

	void Put(Flag flag)
	{
		var lat = (flag.latitude + 90) / 180;
		var lng = (flag.longitude + 180) / 360;
		var u = lng + 0.5f;
		if (u > 1)
		{
			u -= 1;
		}
		var atan = (u - 1) * 2 * Mathf.PI;
		var asin = (lat - 0.5f) * Mathf.PI;
		var y = Mathf.Sin(asin);
		var length = Mathf.Sqrt(1 - y * y);
		var x = Mathf.Cos(atan) * length;
		var z = Mathf.Sin(atan) * length;

		var pos = transform.rotation * new Vector3(x, y, z) * 20;
		//TODO: Add visuals
		flags[flag] = Instantiate(flagPrefab, pos, Quaternion.LookRotation(pos - transform.position), transform).transform;
	}

	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			PutFlagManual();
		}
	}

	void PutFlagManual()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (!Physics.Raycast(ray, out hit)) return;
		
		Quaternion rotation = Quaternion.LookRotation(hit.normal);
		GameObject flag = Instantiate(flagPrefab, hit.point, rotation);
		flag.transform.SetParent(transform);
		//DebugUV(hit.point);
	}

	void DebugUV(Vector3 pos)
	{
		pos = pos / 20; //Scale to one using planet size
		var u = 1f + Mathf.Atan2(pos.z, pos.x) / (2 * Mathf.PI);
		if (u > 1)
		{
			u -= 1;
		}
		var v = 0.5f + Mathf.Asin(pos.y) / Mathf.PI;
		Debug.Log(u + " : " + v);
	}
}
