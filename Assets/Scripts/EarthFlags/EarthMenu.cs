using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthMenu : MonoBehaviour
{
	public static string version = "0.1a";

	public Text versionText;
	public Button startButton;
	public Button settingsButton;

	public ScrollZoom scrollZoom;
	public PutFlag putFlag;
	public DragRotation dragRotation;

	void Start()
	{
		versionText.text = "Version " + version;
	}

	public void OnSettings()
	{
		Debug.Log("Settings");
	}

	public void OnStart()
	{
		Debug.Log("Start");
		//dope animation
		StartCoroutine(FadeAnimation());
		scrollZoom.enabled = true;
		putFlag.enabled = true;
		dragRotation.enabled = true;
	}

	private IEnumerator FadeAnimation()
	{
		Text startText = startButton.GetComponentInChildren<Text>();
		Text settingsText = settingsButton.GetComponentInChildren<Text>();
		for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * 2)
		{
			startText.color = new Color(1, 1, 1, alpha);
			settingsText.color = new Color(1, 1, 1, alpha);
			yield return null;
		}
		startButton.gameObject.SetActive(false);
		settingsButton.gameObject.SetActive(false);
	} 
}
