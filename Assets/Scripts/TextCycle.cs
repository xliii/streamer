using UnityEngine;
using System.Collections.Generic;

public class TextCycle : MonoBehaviour
{
	public TextMesh text;

	public string[] texts;

	private List<string> strings = new List<string>();
	private int current;

	private int customIndex;

	// Use this for initialization
	void Start ()
	{
		strings.AddRange(texts);
		strings.Add("");
		strings.Add("custom");
		customIndex = strings.Count - 1;
		current = 0;
		SetText();
	}

	private void SetText()
	{
		text.text = strings[current % strings.Count];
	}

	private void AppendCustomText(string ch)
	{
		strings[current%strings.Count] += ch;
		SetText();
	}

	private void BackspaceCustomText()
	{
		int i = current % strings.Count;
		if (strings[i].Length == 0) return;

		strings[i] = strings[i].Substring(0, strings[i].Length - 1);
		SetText();
	}

	private void ClearCustomText()
	{
		strings[current%strings.Count] = "";
		SetText();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			current++;
			SetText();
		}

		if (current % strings.Count != customIndex) return;

		if (Input.GetKeyDown(KeyCode.Delete))
		{
			ClearCustomText();
			return;
		}

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			BackspaceCustomText();
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			AppendCustomText(" ");
			return;
		}

		foreach (KeyCode key in KeyCodeMapper.ALL)
		{
			if (Input.GetKeyDown(key))
			{
				AppendCustomText(key.ToChar());
			}
		}
	}
}
