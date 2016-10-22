using UnityEngine;
using System.Collections.Generic;

public class TextCycle : MonoBehaviour
{
	public TextMesh text;

	public string[] texts;

	public KeyCode forward = KeyCode.Mouse0;
	public KeyCode back = KeyCode.Mouse1;

	private List<string> strings = new List<string>();
	private int current;

	private int customIndex = -1;

	private CustomText custom;

	// Use this for initialization
	void Start ()
	{
		custom = GetComponent<CustomText>();
		strings.AddRange(texts);
		strings.Add("");
		if (custom != null)
		{
			strings.Add("custom");
			customIndex = strings.Count - 1;
		}

		current = 0;
		SetText();
	}

	private int i
	{
		get
		{
			return current % strings.Count;
		} 
	}

	private void SetText()
	{
		text.text = i == customIndex ? custom.Text() : strings[i];
	}

	private void Inc()
	{
		current++;
		CustomLock();
	}

	private void Dec()
	{
		current--;
		if (current < 0)
		{
			current += strings.Count;
		}
		CustomLock();
	}

	private void CustomLock()
	{
		if (custom != null)
		{
			custom.locked = i != customIndex;
		}
	}
	
	void Update () {
		if (Input.GetKeyDown(forward))
		{
			Inc();
			SetText();
		}

		if (Input.GetKeyDown(back))
		{
			Dec();
			SetText();
		}

		if (i != customIndex) return;

		SetText();
	}
}
