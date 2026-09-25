using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class Donor
    {
        [Key]
        public int DonorId { get; set; }

        // Links the donor to the ASP.NET Identity user.
        public string? UserId { get; set; }

        [StringLength(200)]
        public string? Organization { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        public bool IsAnonymous { get; set; }

        [StringLength(50)]
        public string? TaxNumber { get; set; }

        [Required]
        [StringLength(3)]
        public string PreferredCurrency { get; set; } = "ZAR";

        public decimal TotalDonated { get; set; }
    }
}