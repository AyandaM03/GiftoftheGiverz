
using System.Text.RegularExpressions;
using GiftoftheGiverz.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftoftheGiverz.Tests
{
    [TestClass]
    public class DonationHelperTests
    {
        [TestMethod]
        public void CalculateDonationFee_ReturnsTwoPercentOfAmount()
        {
            // Arrange
            decimal donationAmount = 500m;

            // Act
            decimal fee = DonationHelper.CalculateDonationFee(donationAmount);

            // Assert
            Assert.AreEqual(10m, fee);
        }

        [TestMethod]
        public void CalculateDonationFee_ReturnsZeroForNonPositiveAmount()
        {
            // Arrange and Act
            decimal zeroFee = DonationHelper.CalculateDonationFee(0m);
            decimal negativeFee = DonationHelper.CalculateDonationFee(-100m);

            // Assert
            Assert.AreEqual(0m, zeroFee);
            Assert.AreEqual(0m, negativeFee);
        }

        [TestMethod]
        public void CalculateDonationTotal_IncludesDonationFee()
        {
            // Arrange
            decimal donationAmount = 500m;

            // Act
            decimal total = DonationHelper.CalculateDonationTotal(donationAmount);

            // Assert
            Assert.AreEqual(510m, total);
        }

        [TestMethod]
        public void GenerateDonationReference_HasCorrectFormat()
        {
            // Act
            string reference = DonationHelper.GenerateDonationReference();

            // Assert
            Assert.IsTrue(
                Regex.IsMatch(reference, @"^GOG-\d{14}-\d{3}$"),
                $"Unexpected donation reference format: {reference}");
        }
    }
}
