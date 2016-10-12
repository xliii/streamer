using UnityEngine;

public class AnimationNone : AnimationProcessor {

	public override void Animate(TextMesh target, string value)
	{
		target.text = value;
	}
}
