using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class Volunteer
    {
        private ReliefProject? reliefProject;

        public int VolunteerId { get; set; }
    
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [Required]
        [StringLength(500)]
        public string Skills { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Availability { get; set; } = string.Empty;

        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}