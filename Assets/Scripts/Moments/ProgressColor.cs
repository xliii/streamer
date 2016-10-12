using UnityEngine;

public class ProgressColor : MonoBehaviour
{
	public TextMesh text;
	public Gradient color;

	public float target;

	private float value;
	
	// Update is called once per frame
	void Update ()
	{
		if (!text) return;

		if (!float.TryParse(text.text, out value))
		{
			Debug.LogError("Could not parse " + text.text + " - not a number");
		}
		text.color = color.Evaluate(value/target);
	}
}
