using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextCycle : MonoBehaviour
{
	public TextMesh text;

	public string[] texts;

	public KeyCode forward = KeyCode.Mouse0;
	public KeyCode back = KeyCode.Mouse1;

	private List<string> strings = new List<string>();
	private int current;

	private int customIndex;

	private const string TIMER_REGEX = "\\[\\d+:?\\d*\\]";

	private double timer; //in seconds
	private int initialTimer;

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
		if (span.Hours > 0)
		{
			return Regex.Replace(value, TIMER_REGEX, string.Format("{0}:{1}:{2:00}", span.Hours, span.Minutes, span.Seconds));
		}
		else
		{
			return Regex.Replace(value, TIMER_REGEX, string.Format("{0}:{1:00}", span.Minutes, span.Seconds));
		}
	}

	private void SetTimer(string value)
	{
		//TODO: Auto trailing zeroes
		Match m = Regex.Match(value, TIMER_REGEX);
		if (!m.Success)
		{
			SetTimer(0);
			return;
		}
		
		string val = m.Value.Substring(1, m.Value.Length - 2); //get rid of []
		int delim = val.IndexOf(":");
		if (delim > 0)
		{
			int minutes = int.Parse(val.Substring(0, delim));
			int seconds = int.Parse(val.Substring(delim + 1, val.Length - delim - 1));			
			SetTimer(minutes*60 + seconds);
		}
		else
		{
			SetTimer(int.Parse(val) * 60); //that's minutes, ok?
		}
	}

	private void SetTimer(int seconds)
	{
		if (initialTimer == seconds) return;

		timer = initialTimer = seconds;
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

	private void Inc()
	{
		current++;
	}

	private void Dec()
	{
		current--;
		if (current < 0)
		{
			current += strings.Count;
		}
	}
	
	void Update () {

		//Update timer
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}
		
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

		foreach (KeyCode key in KeyCodeMapper.ALL)
		{
			if (Input.GetKeyDown(key))
			{
				AppendCustomText(key.ToChar());
			}
		}
	}
}
