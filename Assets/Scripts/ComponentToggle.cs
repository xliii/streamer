using UnityEngine;

public class ComponentToggle : MonoBehaviour
{
	public MonoBehaviour component;
		
	void Update ()
	{
		if (component == null) return;

		if (Input.GetMouseButtonDown(1))
		{
			component.enabled = !component.enabled;
		}
	}
}
