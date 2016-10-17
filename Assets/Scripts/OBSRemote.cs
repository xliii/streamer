﻿using System.Runtime.InteropServices;
using System;
using UnityEngine;
public class OBSRemote {

	public static void SendKey(KeyCode keyCode)
	{
		var obsHandle = WindowsAPI.ObsHandle;
		if (obsHandle == IntPtr.Zero)
		{
			Debug.LogError("No OBS window found :(");
			return;
		}

		Debug.Log ("Sending key to OBS: " + keyCode);
		WindowsAPI.SendKeycode(WindowsAPI.WindowType.OBS, keyCode);
	}
}
