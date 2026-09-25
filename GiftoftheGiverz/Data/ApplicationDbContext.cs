using GiftoftheGiverz.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiverz.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Donor> Donors { get; set; }

        public DbSet<Donation> Donations { get; set; }

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<ReliefProject> ReliefProjects { get; set; }

        public DbSet<ProjectUpdate> ProjectUpdates { get; set; }

        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; }

        public DbSet<TaxCertificate> TaxCertificates { get; set; }


        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // Donation amount
            builder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(18,2)");


            // Donor total donated
            builder.Entity<Donor>()
                .Property(d => d.TotalDonated)
                .HasColumnType("decimal(18,2)");


            // Relief Project → Project Updates
            builder.Entity<ProjectUpdate>()
                .HasOne(p => p.ReliefProject)
                .WithMany(p => p.Updates)
                .HasForeignKey(p => p.ReliefProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            // Donation → Tax Certificate (1-to-1)
            builder.Entity<TaxCertificate>()
                .HasOne(t => t.Donation)
                .WithOne()
                .HasForeignKey<TaxCertificate>(
                    t => t.DonationId)
                .OnDelete(DeleteBehavior.Cascade);


            // Volunteer → Volunteer Assignments
            builder.Entity<VolunteerAssignment>()
                .HasOne(a => a.Volunteer)
                .WithMany()
                .HasForeignKey(a => a.VolunteerId)
                .OnDelete(DeleteBehavior.Cascade);


            // Relief Project → Volunteer Assignments
            builder.Entity<VolunteerAssignment>()
                .HasOne(a => a.ReliefProject)
                .WithMany()
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}