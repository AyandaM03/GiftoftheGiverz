namespace GiftoftheGiverz.Helpers
{
    public static class DonationHelper
    {
        public static string GenerateDonationReference()
        {
            return $"GOG-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}";
        }

        public static decimal CalculateDonationFee(decimal amount)
        {
            if (amount <= 0)
                return 0;

            return Math.Round(amount * 0.02m, 2);
        }

        public static decimal CalculateDonationTotal(decimal amount)
        {
            return amount + CalculateDonationFee(amount);
        }
    }
}