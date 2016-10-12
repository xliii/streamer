using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SocialController : MonoBehaviour
{
	public enum SocialType
	{
		Twitter,
		Facebook,
		Instagram,
		Soundcloud
	}	

	public TextMesh text;
	public SpriteRenderer sprite;

	public SocialType[] socialsEnabled;

	[Header("Images")]
	public Sprite twitter;
	public Sprite facebook;
	public Sprite instagram;
	public Sprite soundcloud;	

	[Header("Configuration")]
	public float fadeDuration = 0.75f;	
	public float rotationDuration = 1f;
	public float onDuration = 2f;
	public float offDuration = 20f;

	public iTween.EaseType easing = iTween.EaseType.easeInOutQuad;

	[Header("Positioning")]
	public Vector3 positionBig = new Vector3(0.91f, 0.91f);
	public Vector3 scaleBig = Vector3.one;

	public Vector3 positionSmall;
	public Vector3 scaleSmall;

	private Color textColor;
	private Color spriteColor;

	private SocialType current;

	private bool rotationOngoing;

	private Dictionary<SocialType, Sprite> sprites;	

	private bool big;

	void Awake ()
	{
		textColor = text.color;
		spriteColor = sprite.color;
		sprites = new Dictionary<SocialType, Sprite>()
		{
			{SocialType.Facebook, facebook},
			{SocialType.Twitter, twitter},
			{SocialType.Instagram, instagram},
			{SocialType.Soundcloud, soundcloud},
		};
		SetSmall();
	}

	void SetBig()
	{
		big = true;
		//transform.position = positionBig;
		//transform.localScale = scaleBig;
	}

	void SetSmall()
	{
		big = false;
		//transform.position = positionSmall;
		//transform.localScale = scaleSmall;
	}

	void Prepare()
	{
		current = SocialType.Twitter;
		sprite.sprite = sprites[current];
		sprite.color = Color.clear;
		text.color = Color.clear;
	}

	IEnumerator Loop()
	{
		Prepare();
		yield return StartCoroutine(FadeIn(fadeDuration));
		yield return new WaitForSeconds(onDuration);
		foreach (SocialType social in socialsEnabled)
		{
			if (social == current) continue;
			
			SwitchTo(social);
			while (rotationOngoing) yield return null;
			yield return new WaitForSeconds(onDuration);
		}		
		yield return StartCoroutine(FadeOut(fadeDuration));
		yield return new WaitForSeconds(offDuration);
		StartCoroutine(Loop());
	}	

	void Start()
	{
		StartCoroutine(Loop());
	}

	void SwitchTo(SocialType social)
	{
		Rotate(social);
	}

	void Rotate(SocialType social)
	{
		rotationOngoing = true;
		iTween.RotateBy(sprite.gameObject, new Hashtable()
		{
			{ "y", 0.25f },
			{ "time", rotationDuration },
			{ "easetype", iTween.EaseType.easeInQuad },
			{ "oncomplete", "Rotate2" },
			{ "oncompleteparams", social },
			{ "oncompletetarget", gameObject }
		});
	}

	void Rotate2(SocialType social)
	{
		sprite.sprite = sprites[social];
		sprite.transform.rotation = Quaternion.Euler(0, -90, 0);
		iTween.RotateBy(sprite.gameObject, new Hashtable()
		{
			{ "y", 0.25f },
			{ "time", rotationDuration },
			{ "easetype", iTween.EaseType.easeOutQuad },
			{ "oncomplete", "FinishRotate" },
			{ "oncompletetarget", gameObject }
		});
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F8))
		{
			if (big)
			{
				SetSmall();
			}
			else
			{
				SetBig();
			}
		}
	}

	void FinishRotate()
	{
		rotationOngoing = false;
	}

	IEnumerator FadeOut(float duration)
	{
		return Fade(duration, false);
	}

	IEnumerator FadeIn(float duration)
	{
		return Fade(duration, true);
	}

	IEnumerator Fade(float duration, bool inc)
	{
		for (float i = 0; i < duration; i += Time.deltaTime)
		{
			float value = inc ? i/duration : 1 - i/duration;
			text.color = new Color(textColor.r, textColor.g, textColor.b, value);
			sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, value);
			yield return null;
		}

		text.color = new Color(textColor.r, textColor.g, textColor.b, inc ? 1 : 0);
		sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, inc ? 1 : 0);
	}
}
