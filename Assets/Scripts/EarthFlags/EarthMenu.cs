using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EarthMenu : MonoBehaviour
{
	public static string version = "0.1a";

	public Text versionText;
	public Button startButton;
	public Button settingsButton;
	public Button quitButton;

	public ScrollZoom scrollZoom;
	public PutFlag putFlag;
	public DragRotation dragRotation;

	public GameObject mainMenu;
	public GameObject settingsMenu;	

	public GameObject badConfigLabel;

	public Image pauseButton;

	void Start()
	{
		versionText.text = "Version " + version;
		if (!Config.Init())
		{
			mainMenu.SetActive(false);
			settingsMenu.SetActive(false);
			badConfigLabel.SetActive(true);
		}
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	public void OnSettings()
	{
		settingsMenu.SetActive(true);
		mainMenu.SetActive(false);
	}

	public void OnExport()
	{
		var json = FlagRepository.Export();
		File.WriteAllText("flags.json", json);
		Debug.Log("Flags exported");
	}

	public void OnImport()
	{
		var json = File.ReadAllText("flags.json");
		FlagRepository.Import(json);
		Debug.Log("Flags imported");
	}

	public void OnBack()
	{
		settingsMenu.SetActive(false);
		mainMenu.SetActive(true);
	}

	public void OnStart()
	{
		StartCoroutine(FadeAnimation(false));
	}

	public void OnPause()
	{
		StartCoroutine(FadeAnimation(true));
	}

	private IEnumerator FadeAnimation(bool on)
	{
		scrollZoom.enabled = !on;
		putFlag.enabled = !on;
		dragRotation.enabled = !on;
		startButton.gameObject.SetActive(true);
		settingsButton.gameObject.SetActive(true);
		pauseButton.gameObject.SetActive(true);
		quitButton.gameObject.SetActive(true);
		Text startText = startButton.GetComponentInChildren<Text>();
		Text settingsText = settingsButton.GetComponentInChildren<Text>();
		Text quitText = quitButton.GetComponentInChildren<Text>();
		for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * 2)
		{
			float a = on ? 1 - alpha : alpha;
			startText.color = new Color(1, 1, 1, a);
			settingsText.color = new Color(1, 1, 1, a);
			quitText.color = new Color(1, 1, 1, a);
			pauseButton.color = new Color(1, 1, 1, 1 - a);
			yield return null;
		}
		startButton.gameObject.SetActive(on);
		settingsButton.gameObject.SetActive(on);
		quitButton.gameObject.SetActive(on);
		pauseButton.gameObject.SetActive(!on);
	} 
}
