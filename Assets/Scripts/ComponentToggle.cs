using UnityEngine;
using UnityEngine.UI;

public class ComponentToggle : MonoBehaviour
{
	public MonoBehaviour component;
	public KeyCode key = KeyCode.None;

	public bool colorMode = false;
		
	void Update ()
	{
		if (component == null) return;

		if (key != KeyCode.None && Input.GetKeyDown(key))
		{
			Toggle();
		}
	}

	public void Toggle()
	{
		if (colorMode)
		{
			Text text = component as Text;
			if (text != null)
			{
				text.color = new Color(text.color.r, text.color.g, text.color.b, 1 - text.color.a);
			}
		}
		else
		{
			component.enabled = !component.enabled;
		}
	}
}
