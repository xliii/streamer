using UnityEngine;

public class ActiveWindowListener : MonoBehaviour
{
	private WindowsAPI.WindowType activeWindow = WindowsAPI.WindowType.Unknown;
	
	public KeyCode unity;
	public KeyCode visualStudio;
	public KeyCode obs;

	// Update is called once per frame
	void Update ()
	{
		WindowsAPI.WindowType active = WindowsAPI.ActiveWindow();
		if (activeWindow != active)
		{
			activeWindow = active;
			KeyCode key = ActiveKeyCode();
			if (key != KeyCode.None)
			{
				OBSRemote.SendKey(key);
			}
		}
	}

	KeyCode ActiveKeyCode()
	{
		switch (activeWindow)
		{
			case WindowsAPI.WindowType.Unity:
				return unity;
			case WindowsAPI.WindowType.VisualStudio:
				return visualStudio;
			case WindowsAPI.WindowType.OBS:
				return obs;
			default:
				return KeyCode.None;
		}
	}
}
