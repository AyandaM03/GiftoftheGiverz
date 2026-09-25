using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int VolunteerId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? Role { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Assigned";

        public string? AssignedBy { get; set; }

        public Volunteer? Volunteer { get; set; }

        public ReliefProject? ReliefProject { get; set; }
    }
}