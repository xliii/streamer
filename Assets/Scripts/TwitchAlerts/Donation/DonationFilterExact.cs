using System.Linq;

public class DonationFilterExact : DonationAmountFilter
{
	public float[] amounts;

	public override bool Matches(float amount)
	{
		if (amounts == null) return false;

		return amounts.Any(trigger => trigger == amount);
	}
}