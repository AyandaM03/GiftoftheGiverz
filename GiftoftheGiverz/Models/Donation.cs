using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class Donation
    {
        public int DonationId { get; set; }

        // Links the donation to the logged-in donor.
        public string? UserId { get; set; }

        [Required]
        [Range(1, 100000000)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "One-Time";

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public bool IsAnonymous { get; set; }

        public string Status { get; set; } = "Confirmed";

        public string ReferenceNumber { get; set; } = string.Empty;

        public string? RecurringFrequency { get; set; }
    }
}