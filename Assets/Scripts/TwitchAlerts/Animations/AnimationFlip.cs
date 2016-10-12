using UnityEngine;
using System.Collections;

public abstract class AnimationFlip : AnimationProcessor
{
	protected abstract string GetAxis();

	public override void Animate(TextMesh target, string value)
	{
		RotateLayoutText(target, value);
	}	

	void RotateLayoutText(TextMesh textMesh, string message)
	{
		iTween.RotateBy(textMesh.gameObject, new Hashtable()
		{
			{ GetAxis(), 0.25f },
			{ "time", 1 },
			{ "easetype", iTween.EaseType.easeInQuad },
			{ "oncomplete", "RotateLayoutText2" },
			{ "oncompleteparams", new LayoutTextParams()
			{
				text = textMesh,
				message = message
			} },
			{ "oncompletetarget", gameObject }
		});
	}

	void RotateLayoutText2(LayoutTextParams args)
	{
		args.text.text = args.message;
		args.text.transform.rotation = GetMiddleRotation();
		iTween.RotateBy(args.text.gameObject, new Hashtable()
		{
			{ GetAxis(), 0.25f },
			{ "time", 1 },
			{ "easetype", iTween.EaseType.easeOutQuad },
			{ "oncomplete", "FinishRotate" },
			{ "oncompletetarget", gameObject }
		});
	}

	Quaternion GetMiddleRotation()
	{
		switch (GetAxis())
		{
			case "x": return Quaternion.Euler(-90, 0, 0);
			case "y": return Quaternion.Euler(0, -90, 0);
			case "z": return Quaternion.Euler(0, 0, -90);
			default: return Quaternion.identity;
		}
	}

	class LayoutTextParams
	{
		public TextMesh text;
		public string message;
	}
}
