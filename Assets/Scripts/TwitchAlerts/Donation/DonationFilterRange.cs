public class DonationFilterRange : DonationAmountFilter
{

	public float from;
	public float to;

	public override bool Matches(float amount)
	{
		return amount >= from && amount < to;
	}
}
