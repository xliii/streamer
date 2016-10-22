using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class CustomText : MonoBehaviour
{
	[SerializeField]
	private string text;

	public bool locked;

	private const string TIMER_REGEX = "\\[\\d+:?\\d*\\]";

	private double timer; //in seconds
	private int initialTimer;

	public string Text()
	{
		return ApplyTimer(text);
	}

	void Update () {
		//Update timer
		if (timer > 0)
		{
			timer -= Time.deltaTime;
		}

		if (locked) return;

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

	private void BackspaceCustomText()
	{
		if (text.Length == 0) return;

		text = text.Substring(0, text.Length - 1);
	}

	private void ClearCustomText()
	{
		text = "";
	}

	private void AppendCustomText(string ch)
	{
		text += ch;
		SetTimer(text);
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
			SetTimer(minutes * 60 + seconds);
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
}
