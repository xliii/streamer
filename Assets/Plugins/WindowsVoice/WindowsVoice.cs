using UnityEngine;
using System.Runtime.InteropServices;

public class WindowsVoice : MonoBehaviour {
	[DllImport("WindowsVoice")]
	public static extern void initSpeech();
	[DllImport("WindowsVoice")]
	public static extern void destroySpeech();
	[DllImport("WindowsVoice")]
	public static extern void addToSpeechQueue( string s);

	public static WindowsVoice theVoice = null;

	// Use this for initialization
	void Start () {
		if (theVoice == null)
		{
			theVoice = this;
			initSpeech();
		}
	}

	public void Speak(string message) {
		addToSpeechQueue(message);
	}

	void OnDestroy()
	{
		if (theVoice == this)
		{
			destroySpeech();
			theVoice = null;
		}
	}
}
