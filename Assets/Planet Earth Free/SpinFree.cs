using UnityEngine;
using System.Collections;

/// <summary>
/// Spin the object at a specified speed
/// </summary>
public class SpinFree : MonoBehaviour {
	[Tooltip("Spin: Yes or No")]
	public bool spin;
	public float speed = 10f;

	[HideInInspector]
	public bool clockwise = true;
	[HideInInspector]
	public float direction = 1f;
	[HideInInspector]
	public float directionChangeSpeed = 2f;

	private DragRotation drag;

	void Start()
	{
		drag = GetComponent<DragRotation>();
	}

	// Update is called once per frame
	void Update()
	{
		if (drag != null && drag.isActiveAndEnabled && Input.GetMouseButton(0)) return;

		if (direction < 1f) {
			direction += Time.deltaTime / (directionChangeSpeed / 2);
		}

		if (spin) {
			if (clockwise) {
				transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
			} else {
				transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
			}
		}
	}
}