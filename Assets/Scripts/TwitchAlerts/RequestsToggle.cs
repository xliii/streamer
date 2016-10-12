using UnityEngine;
using System.Collections;

public class RequestsToggle : MonoBehaviour
{
	public GameObject requestsOn;
	public GameObject requestsOff;

	public void Toggle()
	{
		requestsOff.SetActive(!requestsOff.activeSelf);
		requestsOn.SetActive(!requestsOn.activeSelf);
	}
}
