using UnityEngine;
using System.Collections;

public class ScaleTween : MonoBehaviour
{
	[Range(0, 1)]
	public float amount = 0.02f;

	[Range(0, 10)]
	public float duration = 2f;

	public iTween.EaseType easeType = iTween.EaseType.easeInOutSine;

	Vector3 baseScale;

	// Use this for initialization
	void Start ()
	{
		baseScale = transform.localScale;
		transform.localScale = From();
		ScaleTo();
	}

	Vector3 From()
	{
		return baseScale * (1 - amount);
	}

	Vector3 To()
	{
		return baseScale * (1 + amount);
	}

	void ScaleTo()
	{
		iTween.ScaleTo(gameObject, new Hashtable()
		{
			{ "scale", To() },
			{ "time", duration },
			{ "oncomplete", "ScaleFrom"},
			{ "easetype", easeType}
		});
	}

	void ScaleFrom()
	{
		iTween.ScaleTo(gameObject, new Hashtable()
		{
			{ "scale", From() },
			{ "time", duration },
			{ "oncomplete", "ScaleTo"},
			{ "easetype", easeType}
		});
	}
}
