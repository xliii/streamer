using UnityEngine;

public class ComponentToggle : MonoBehaviour
{
	public MonoBehaviour component;
	public KeyCode key = KeyCode.None;
		
	void Update ()
	{
		if (component == null) return;

		if (key != KeyCode.None && Input.GetKeyDown(key))
		{
			component.enabled = !component.enabled;
		}
	}
}
