using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class TaxCertificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        public int DonationId { get; set; }

        [Required]
        [StringLength(50)]
        public string CertificateNumber { get; set; } = string.Empty;

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        public string? IssuedBy { get; set; }

        [StringLength(500)]
        public string? PdfLink { get; set; }

        public Donation? Donation { get; set; }
    }
}