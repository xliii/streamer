using UnityEngine;
using System.Collections.Generic;

public class TextCycle : MonoBehaviour
{
	public TextMesh text;

	public string[] texts;

	private List<string> strings = new List<string>();
	private int current;

	private KeyCode[] letters =
	{
		KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
		KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
		KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
		KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
		KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
		KeyCode.Z,
	};

	private KeyCode[] digits =
	{
		KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
		KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
	};

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

		foreach (KeyCode key in letters)
		{
			if (Input.GetKeyDown(key))
			{
				AppendCustomText(key.ToString());
				return;
			}
		}

		foreach (KeyCode key in digits)
		{
			if (Input.GetKeyDown(key))
			{
				AppendCustomText(key.ToString().Substring(5));
				return;
			}
		}
	}
}
