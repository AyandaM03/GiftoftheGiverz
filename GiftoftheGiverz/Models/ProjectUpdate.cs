using System.ComponentModel.DataAnnotations;

namespace GiftoftheGiverz.Models
{
    public class ProjectUpdate
    {
        private ReliefProject? reliefProject;

        public int ProjectUpdateId { get; set; }

        public int ReliefProjectId { get; set; }

        public ReliefProject? ReliefProject { get => reliefProject; set => reliefProject = value; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; } = DateTime.Now;

        public string? PostedByUserId { get; set; }
    }
}