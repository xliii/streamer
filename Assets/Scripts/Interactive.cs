using UnityEngine;

public class Interactive : MonoBehaviour
{
	private bool dragged = false;

	public Renderer[] renderers;
	public bool visible = true;

	private static Camera uiCamera;

	private Vector3 offset;	

	void Start()
	{
		if (uiCamera == null)
		{
			uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
		}
	}

	bool Hits()
	{		
		var mouse = Mouse();
		mouse.z = transform.position.z;		
		return GetComponent<Collider>().bounds.Contains(mouse);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && Hits())
		{
			dragged = true;
			offset = transform.position - Mouse();
		} else if (Input.GetMouseButtonUp(0))
		{
			dragged = false;
			offset = Vector3.zero;
		}

		if (Input.GetMouseButton(0) && dragged)
		{
			transform.position = Mouse() + offset;
		}

		if (Input.GetMouseButtonDown(1))
		{
			ToggleVisible();
		}
	}

	void ToggleVisible()
	{
		visible = !visible;
		if (renderers == null || renderers.Length == 0) return;

		foreach (Renderer r in renderers)
		{
			r.enabled = visible;
		}
	}

	Vector3 Mouse()
	{		
		Vector3 mouse = uiCamera.ScreenToWorldPoint(Input.mousePosition);
		mouse.z = 0;
		return mouse;
	}
}
