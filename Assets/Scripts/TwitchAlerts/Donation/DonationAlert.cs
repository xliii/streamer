using System.Collections;
using System.Linq;
using UnityEngine;

public class DonationAlert : Alert
{
	private float averageWordsPerSecond = 2.166f;

	[Header("Donation")]
	public TextMeshWrapper message;
	public ParticleIntensityTune particlesTuning;

	private DonationAmountFilter[] filters;

	public bool Matches(float amount)
	{
		if (filters == null || filters.Length == 0) return false;

		return filters.Any(filter => filter.Matches(amount));
	}

	protected override void Start()
	{
		base.Start();
		message.text = "";
		filters = GetComponents<DonationAmountFilter>();
	}

	protected override IEnumerator ProcessAlert(AlertData data)
	{
		AlertManager.alertInProgress = true;
		if (sound && audioSource)
		{
			audioSource.PlayOneShot(sound);
		}
		StartCoroutine(ParticleCoroutine());
		SetLayoutText(data);
		SetContent(data);
		
		yield return new WaitForSeconds(duration);
		alertText.text = "";
		message.text = "";
	}

	protected override void SetContent(AlertData data)
	{
		DonationAlertData donationData = data as DonationAlertData;
		if (particlesTuning)
		{
			if (donationData.amount > 0)
			{
				particlesTuning.SetRate(donationData.amount);
			}
			else
			{
				particlesTuning.SetRate(ParticleIntensityTune.DEFAULT_RATE);
			}
		}

		alertText.text = donationData.username + " just donated " + donationData.amountFormatted;
		message.text = donationData.message;
		StartCoroutine(VoiceCoroutine(donationData.message, sound.length));
	}

	protected override void SetLayoutText(AlertData data)
	{
		DonationAlertData donationData = data as DonationAlertData;
		string message = string.Format("{0} ({1})", donationData.username, donationData.amountFormatted);
		SetLayoutText(layoutText, message);
	}

	IEnumerator VoiceCoroutine(string toSpeak, float delay)
	{
		yield return new WaitForSeconds(delay);

		WindowsVoice.theVoice.Speak(toSpeak);
		yield return new WaitForSeconds(EstimateDuration(toSpeak) + 1);
		AlertManager.alertInProgress = false;
	}

	float EstimateDuration(string toSpeak)
	{
		int words = toSpeak.Split(' ').Length;
		float estimation = words / averageWordsPerSecond;
		return estimation;
	}

	protected override TwitchAlertsType Type()
	{
		return TwitchAlertsType.most_recent_donator;
	}
}
