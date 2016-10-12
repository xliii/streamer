using System.Runtime.InteropServices;
using System;
using UnityEngine;

public class OBSRemote {

	private const UInt32 WM_KEYDOWN = 0x0100;

	private static IntPtr obsHwnd;

	[DllImport("user32.dll", EntryPoint="FindWindow", SetLastError = true)]
	private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

	[DllImport("user32.dll")]
	private static extern bool PostMessage (IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

	private static void InitObsHandler()
	{
		obsHwnd = FindWindowByCaption(IntPtr.Zero, "OBS 0.16.2 (windows) - Profile: Untitled - Scenes: Untitled");		
		if (obsHwnd != IntPtr.Zero)
		{
			Debug.Log("OBS Window Handle retrieved");
		}
		else
		{
			Debug.LogError("Couldn't retrieve OBS Window Handle");
		}
	}	

	public static void SendKey(KeyCode keyCode) {
		if (obsHwnd == IntPtr.Zero)
		{
			InitObsHandler();
		}

		Debug.Log ("Sending key to OBS: " + keyCode);
		PostMessage (obsHwnd, WM_KEYDOWN, keyCode.ToVirtualKey(), 0);
	}
}
