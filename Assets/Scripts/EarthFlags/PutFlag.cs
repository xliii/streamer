﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutFlag : MonoBehaviour
{
	public GameObject viewerFlag;
	public GameObject streamerFlag;
	public GameObject modFlag;
	public GameObject subFlag;

	public ParticleSystem flagCreate;

	public AudioClip[] createSounds;

	private Dictionary<UserRole, GameObject> prefabByRole = new Dictionary<UserRole, GameObject>();

	private Dictionary<Flag, GameObject> flags = new Dictionary<Flag, GameObject>();

	private AudioSource audioSource;

	private int manualIndex = 0;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		prefabByRole[UserRole.Viewer] = viewerFlag;
		prefabByRole[UserRole.Mod] = modFlag;
		prefabByRole[UserRole.Streamer] = streamerFlag;
		prefabByRole[UserRole.Subscriber] = subFlag;
	}

	void Start()
	{
		Messenger.AddListener<Flag>(Flag.FLAG_ADDED, AddWithEffect);
		Messenger.AddListener<Flag>(Flag.FLAG_UPDATED, UpdateFlag);
		Messenger.AddListener<Flag>(Flag.FLAG_REMOVED, RemoveFlag);
		Messenger.AddListener(Flag.FLAGS_CLEARED, Clear);
		AddFlags();
	}

	void Clear()
	{
		foreach (GameObject flag in flags.Values)
		{
			//TODO: Add visuals
			Destroy(flag);
		}

		flags.Clear();
	}

	void Sound()
	{
		AudioClip clip = createSounds[Random.Range(0, createSounds.Length)];
		audioSource.PlayOneShot(clip);
	}

	void Particles(Vector3 pos)
	{
		if (flagCreate != null)
		{
			Instantiate(flagCreate, pos, Quaternion.identity, transform);
		}
	}

	void UpdateFlag(Flag flag)
	{
		if (!flags.ContainsKey(flag))
		{
			Debug.LogError("Flag not in dictionary: " + flag);
		}

		GameObject flagVisual = flags[flag];
		var pos = getPos(flag);
		flagVisual.transform.position = pos;
		flagVisual.transform.rotation = Quaternion.LookRotation(pos - transform.position);
	}

	void RemoveFlag(Flag flag)
	{
		if (!flags.ContainsKey(flag))
		{
			Debug.LogError("Flag not in dictionary: " + flag);
		}

		GameObject flagVisual = flags[flag];
		flags.Remove(flag);
		//TODO: Add visuals
		Destroy(flagVisual);
	}

	void AddFlags()
	{
		var all = FlagRepository.GetAll();
		Debug.Log("Adding " + all.Count + " flags");
		foreach (Flag flag in all)
		{
			Add(flag);
		}
	}

	void AddWithEffect(Flag flag)
	{
		var flagVisual = Add(flag);
		Sound();
		Particles(flagVisual.transform.position);
	}

	GameObject Add(Flag flag)
	{
		var pos = getPos(flag);
		var user = UserRepository.GetByUsername(flag.user);
		UserRole role;
		if (user == null)
		{
			if (!flag.user.StartsWith("Debug"))
			{
				Debug.LogWarning("Could not retrieve user when adding flag: " + flag.user);
			}
			
			role = UserRole.Viewer;
		}
		else
		{
			role = user.GetBestRole();
		}
		if (!prefabByRole.ContainsKey(role))
		{
			Debug.LogError("No flag available for role: " + role + " - fallback to viewer");
			role = UserRole.Viewer;
		}
		var flagVisual = Instantiate(prefabByRole[role], pos, Quaternion.LookRotation(pos - transform.position), transform).gameObject;
		flags[flag] = flagVisual;
		return flagVisual;
	}

	Vector3 getPos(Flag flag)
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
		return transform.rotation * new Vector3(x, y, z) * 20;
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
		GameObject flag = Instantiate(viewerFlag, hit.point, rotation);
		flag.transform.SetParent(transform);
		flags[new Flag("Manual" + (manualIndex++))] = flag;
		Particles(hit.point);
		Sound();
	}

	public void SetColor(UserRole role, Color color)
	{
		Debug.Log("Putting color " + color + " for role " + role);
		GameObject flag = prefabByRole[role];
		if (flag == null)
		{
			Debug.LogWarning("No flag for role: " + role);
			return;
		}

		Renderer r = flag.GetComponentInChildren<SkinnedMeshRenderer>();
		r.sharedMaterial.color = color;
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
