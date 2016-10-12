using UnityEngine;
using System.Collections;

public abstract class DonationAmountFilter : MonoBehaviour
{
	public abstract bool Matches(float amount);
}
