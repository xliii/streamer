using UnityEngine;

public abstract class AnimationProcessor : MonoBehaviour
{
	public abstract void Animate(TextMesh target, string value);
}
