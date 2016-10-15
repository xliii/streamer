using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextCycle : MonoBehaviour
{
	public TextMesh text;

	public string[] texts;

	private List<string> strings = new List<string>();
	private int current;

	private int customIndex;

	private const string TIMER_REGEX = "\\[\\d\\d:\\d\\d\\]";

	private double timer; //in seconds

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

	private int i
	{
		get
		{
			return current % strings.Count;
		} 
	}

	private void SetText()
	{
		text.text = ApplyTimer(strings[i]);
	}

	private void AppendCustomText(string ch)
	{
		strings[i] += ch;
		SetTimer(strings[i]);
		SetText();
	}

	private string ApplyTimer(string value)
	{
		Match m = Regex.Match(value, TIMER_REGEX);
		if (!m.Success) return value;

		TimeSpan span = TimeSpan.FromSeconds(timer);
		return Regex.Replace(value, TIMER_REGEX, string.Format("{0}:{1}", span.Minutes, span.Seconds));
	}

	private void SetTimer(string value)
	{
		//TODO: Auto trailing zeroes
		Match m = Regex.Match(value, TIMER_REGEX);
		if (!m.Success) return;
		
		string val = m.Value.Substring(1, m.Value.Length - 2);
		DateTime dateTime = DateTime.ParseExact(val, "mm:ss", System.Globalization.CultureInfo.InvariantCulture);
		timer = (dateTime - dateTime.Date).TotalSeconds;
	}
		 

	private void BackspaceCustomText()
	{
		if (strings[i].Length == 0) return;

		strings[i] = strings[i].Substring(0, strings[i].Length - 1);
		SetText();
	}

	private void ClearCustomText()
	{
		strings[i] = "";
		SetText();
	}
	
	void Update () {

		//Update timer
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}

		if (Input.GetMouseButtonDown(0))
		{
			current++;
			SetText();
		}

		if (i != customIndex) return;

		SetText();

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
