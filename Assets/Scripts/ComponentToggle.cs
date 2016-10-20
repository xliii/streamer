using UnityEngine;

public class ComponentToggle : MonoBehaviour
{
	private const int NONE = -1;

	public MonoBehaviour component;

	[Header("-1 if None")]
	public int mouseButton = NONE;
	public KeyCode key = KeyCode.None;
		
	void Update ()
	{
		if (component == null) return;

		if (key != KeyCode.None && Input.GetKeyDown(key))
		{
			Toggle();
		}

		if (mouseButton != NONE && Input.GetMouseButtonDown(mouseButton))
		{
			Toggle();
		}
	}

	void Toggle()
	{
		component.enabled = !component.enabled;
	}
}
