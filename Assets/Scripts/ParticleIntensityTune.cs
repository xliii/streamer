using UnityEngine;
using System.Collections;

public class ParticleIntensityTune : MonoBehaviour
{
	public ParticleSystem particles;

	private ParticleSystem.EmissionModule emission;

	public static readonly float DEFAULT_RATE = 15f;

	public static readonly float MIN_RATE = 5f;

	void Awake()
	{
		emission = particles.emission;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(GetRate() + 5);
		}

		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			emission.rateOverTime = new ParticleSystem.MinMaxCurve(GetRate() - 5);
		}
		if (Input.GetKeyDown(KeyCode.KeypadMultiply))
		{
			if (particles.isPlaying)
			{
				particles.Stop();
				particles.Clear();
			}
			else
			{
				particles.Play();
			}
		}
	}

	private float GetRate()
	{
		return emission.rateOverTime.constantMax;
	}

	public void SetRate(float rate)
	{
		emission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Max(MIN_RATE, rate));
	}

	public void Stop(bool clear = false)
	{
		particles.Stop();
		if (clear)
		{
			particles.Clear();
		}
	}

	public void Play()
	{
		particles.Play();
	}
}
