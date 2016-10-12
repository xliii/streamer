using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class HueShift : MonoBehaviour
{
	private TextMesh text;

	public float speed = 0.2f;

	private float h;
	private float s;
	private float v;

	void Start()
	{
		text = GetComponent<TextMesh>();
		if (!text)
		{
			Debug.LogError("HueShift init error: TextMesh not set in GameObject " + name);
			return;
		}
		Color.RGBToHSV(text.color, out h, out s, out v);
	}
	
	void Update ()
	{
		if (!text) return;

		h += Time.deltaTime * speed;
		if (h > 1)
		{
			h -= 1;
		}
		text.color = Color.HSVToRGB(h, s, v);
	}
}
