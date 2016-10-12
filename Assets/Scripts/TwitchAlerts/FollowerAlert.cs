using System.Collections;
using UnityEngine;

public class FollowerAlert : Alert
{
	[Header("Follower")]
	public TextMeshWrapper followerCount;

	private int followers;

	protected override void Start()
	{
		base.Start();
		followers = int.Parse(TextFromFile.ReadOnce(TwitchAlertsType.session_follower_count));
		if (followerCount)
		{
			followerCount.text = followers.ToString();
		}
	}

	protected override void SetContent(AlertData data)
	{
		//alertText.text = data.username + " just followed!";
		alertText.text = data.username;
		followers++;
		if (followerCount)
		{
			followerCount.text = followers.ToString();
		}
	}

	protected override TwitchAlertsType Type()
	{
		return TwitchAlertsType.most_recent_follower;
	}

	protected override IEnumerator ParticleCoroutine()
	{
		if (particles == null) yield break;

		particles.Emit(1);
	}
}
