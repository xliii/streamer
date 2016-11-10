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

	public ScrollZoom scrollZoom;
	public PutFlag putFlag;
	public DragRotation dragRotation;

	public GameObject mainMenu;
	public GameObject settingsMenu;

	void Start()
	{
		versionText.text = "Version " + version;
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
