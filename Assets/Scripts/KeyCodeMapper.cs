using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class KeyCodeMapper {

	public static KeyCode[] LETTERS =
	{
		KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
		KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
		KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
		KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
		KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
		KeyCode.Z,
	};

	public static KeyCode[] DIGITS =
	{
		KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
		KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
	};

	public static KeyCode[] SPECIAL =
	{
		KeyCode.LeftParen, KeyCode.RightParen, KeyCode.LeftBracket, KeyCode.RightBracket
	};

	public static KeyCode[] ALL = LETTERS.Concat(DIGITS).Concat(SPECIAL).ToArray();

	public static string ToChar(this KeyCode keyCode)
	{
		if (LETTERS.Contains(keyCode))
		{
			return keyCode.ToString();
		}

		if (DIGITS.Contains(keyCode))
		{
			return keyCode.ToString().Substring(5);
		}

		switch (keyCode)
		{
			case KeyCode.LeftParen:
				return "(";
			case KeyCode.RightParen:
				return ")";
			case KeyCode.LeftBracket:
				return "[";
			case KeyCode.RightBracket:
				return "]";
		}

		return "";
	}

	public static int ToVirtualKey(this KeyCode keyCode)
	{
		switch (keyCode)
		{
			//Letters
			case KeyCode.A: return 0x41;
			case KeyCode.B: return 0x42;
			case KeyCode.C: return 0x43;
			case KeyCode.D: return 0x44;
			case KeyCode.E: return 0x45;
			case KeyCode.F: return 0x46;
			case KeyCode.G: return 0x47;
			case KeyCode.H: return 0x48;
			case KeyCode.I: return 0x49;
			case KeyCode.J: return 0x4A;
			case KeyCode.K: return 0x4B;
			case KeyCode.L: return 0x4C;
			case KeyCode.M: return 0x4D;
			case KeyCode.N: return 0x4E;
			case KeyCode.O: return 0x4F;
			case KeyCode.P: return 0x50;
			case KeyCode.Q: return 0x51;
			case KeyCode.R: return 0x52;
			case KeyCode.S: return 0x53;
			case KeyCode.T: return 0x54;
			case KeyCode.U: return 0x55;
			case KeyCode.V: return 0x56;
			case KeyCode.W: return 0x57;
			case KeyCode.X: return 0x58;
			case KeyCode.Y: return 0x59;
			case KeyCode.Z: return 0x5A;

			//Numpad
			case KeyCode.Keypad0: return 0x60;
			case KeyCode.Keypad1: return 0x61;
			case KeyCode.Keypad2: return 0x62;
			case KeyCode.Keypad3: return 0x63;
			case KeyCode.Keypad4: return 0x64;
			case KeyCode.Keypad5: return 0x65;
			case KeyCode.Keypad6: return 0x66;
			case KeyCode.Keypad7: return 0x67;
			case KeyCode.Keypad8: return 0x68;
			case KeyCode.Keypad9: return 0x69;

			//Fs
			case KeyCode.F1: return 0x70;
			case KeyCode.F2: return 0x71;
			case KeyCode.F3: return 0x72;
			case KeyCode.F4: return 0x73;
			case KeyCode.F5: return 0x74;
			case KeyCode.F6: return 0x75;
			case KeyCode.F7: return 0x76;
			case KeyCode.F8: return 0x77;
			case KeyCode.F9: return 0x78;
			case KeyCode.F10: return 0x79;
			case KeyCode.F11: return 0x7A;
			case KeyCode.F12: return 0x7B;
			case KeyCode.F13: return 0x7C;
			case KeyCode.F14: return 0x7D;
			case KeyCode.F15: return 0x7E;

			//Misc
			case KeyCode.Return: return 0x0D;

			default:
				Debug.Log("No virtual key mapping found for KeyCode: " + keyCode);
				return 0;
		}
	}
}
