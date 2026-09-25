using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class ReliefProject
    {
        public int ReliefProjectId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Planned";

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<ProjectUpdate> Updates { get; set; } = new();
    }
}