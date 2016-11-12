using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostAlert : Alert {
	protected override void SetContent(AlertData data)
	{
		HostAlertData hostData = data as HostAlertData;
		alertText.text = hostData.username + " hosted with " + hostData.viewers + " viewers";
	}

	protected override TwitchAlertsType Type()
	{
		return TwitchAlertsType.host_alert;
	}
}
