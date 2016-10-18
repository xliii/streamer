using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WindowsAPI {

	public enum WindowType
	{
		Unity,
		VisualStudio,
		OBS,
		Unknown
	}

	private static IntPtr handle(this WindowType type)
	{
		switch (type)
		{
			case WindowType.Unity:
				return UnityHandle;
			case WindowType.VisualStudio:
				return VisualStudioHandle;
			case WindowType.OBS:
				return ObsHandle;
			default:
				Debug.LogError("Unknown window type: " + type);
				return IntPtr.Zero;
		}
	}

	public static WindowType ActiveWindow()
	{
		IntPtr active = GetForegroundWindow();
		if (active == IntPtr.Zero)
		{
			return WindowType.Unknown;
		}
		if (active == UnityHandle)
		{
			return WindowType.Unity;
		}

		if (active == VisualStudioHandle)
		{
			return WindowType.VisualStudio;
		}

		if (active == ObsHandle)
		{
			return WindowType.OBS;
		}

		return WindowType.Unknown;
	}	

	private const UInt32 WM_KEYDOWN = 0x0100;

	[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
	private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

	[DllImport("user32.dll")]
	private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	private static extern IntPtr GetForegroundWindow();

	public static IntPtr UnityHandle
	{
		get
		{
			var scene = SceneManager.GetActiveScene();
			var processName = string.Format("Unity 5.5.0b1 Personal (64bit) - {0}.unity - streamer - PC, Mac & Linux Standalone{1} <DX11>", scene.name, scene.isDirty ? "*" : "");
			var handle = FindWindowByCaption(IntPtr.Zero, processName);
			return handle;
		}
	}

	public static IntPtr VisualStudioHandle
	{
		get
		{
			return FindWindowByCaption(IntPtr.Zero, "streamer - Microsoft Visual Studio");
		}
	}

	public static IntPtr ObsHandle
	{
		get
		{
			return FindWindowByCaption(IntPtr.Zero, "OBS 0.16.2 (64bit, windows) - Profile: Untitled - Scenes: Untitled");
		}
	}

	public static void SendKeycode(WindowType type, KeyCode keyCode)
	{
		PostMessage(type.handle(), WM_KEYDOWN, keyCode.ToVirtualKey(), 0);
	}
}
