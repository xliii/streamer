public class DonationFilterAny : DonationAmountFilter {

	public override bool Matches(float amount)
	{
		return true;
	}
}
