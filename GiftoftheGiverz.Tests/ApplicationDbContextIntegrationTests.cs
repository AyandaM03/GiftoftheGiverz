
using System;
using System.Threading.Tasks;
using GiftoftheGiverz.Data;
using GiftoftheGiverz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftoftheGiverz.Tests
{
    [TestClass]
    public class ApplicationDbContextIntegrationTests
    {
        [TestMethod]
        public async Task ApplicationDbContext_CanSaveAndRetrieveDonor()
        {
            //create a unique database for the test.
            string databaseName =
                $"GiftoftheGiverz_Test_{Guid.NewGuid():N}";

            string connectionString =
                $@"Server=(localdb)\MSSQLLocalDB;
                   Database={databaseName};
                   Trusted_Connection=True;
                   MultipleActiveResultSets=true;
                   TrustServerCertificate=True";

            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(connectionString)
                    .Options;

            try
            {
                // create the database and save a donor.
                using (var context = new ApplicationDbContext(options))
                {
                    await context.Database.EnsureCreatedAsync();

                    var donor = new Donor
                    {
                        Organization = "Integration Test Donor",
                        ContactPerson = "Test Contact",
                        PreferredCurrency = "ZAR",
                        TotalDonated = 500m,
                        IsAnonymous = false
                    };

                    context.Donors.Add(donor);

                    await context.SaveChangesAsync();

                    Assert.IsTrue(donor.DonorId > 0,
                        "The donor should receive a database ID.");
                }

                // use a new context to retrieve the saved record.
                using (var verificationContext =
                    new ApplicationDbContext(options))
                {
                    var savedDonor = await verificationContext.Donors
                        .AsNoTracking()
                        .SingleOrDefaultAsync(d =>
                            d.Organization == "Integration Test Donor");

                    Assert.IsNotNull(savedDonor,
                        "The saved donor should be retrieved.");

                    Assert.AreEqual(
                        "Test Contact", savedDonor!.ContactPerson);

                    Assert.AreEqual(
                        "ZAR", savedDonor.PreferredCurrency);

                    Assert.AreEqual(
                        500m, savedDonor.TotalDonated);
                }
            }
            finally
            {
                // Clean up only this test's uniquely named database.
                using var cleanupContext =
                    new ApplicationDbContext(options);

                await cleanupContext.Database.EnsureDeletedAsync();
            }
        }
    }
}
