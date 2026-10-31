using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GiftoftheGiverz.UITests;

[TestClass]
public class DonationTests
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;
    private const string BaseUrl = "https://localhost:7162";
    private const string CreateUrl = BaseUrl + "/Donor/Donations/Create";

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AcceptInsecureCertificates = true;
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

        _driver = new ChromeDriver(options);
        _driver.Manage().Window.Maximize();
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [TestCleanup]
    public void Cleanup()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    // CHANGED: replaces SelectElement. Clicks the <option> with the given value
    // inside the dropdown that has the given id.
    private void ChooseOption(string selectId, string optionValue)
    {
        _driver.FindElement(
            By.CssSelector($"#{selectId} option[value='{optionValue}']")).Click();
    }

    // Test 1: anonymous guest, once-off donation in ZAR.
    [TestMethod]
    public void AnonymousGuestDonation_OneTimeZar_ShowsConfirmation()
    {
        _driver.Navigate().GoToUrl(CreateUrl);

        _driver.FindElement(By.Id("Donation_Amount")).SendKeys("250");
        ChooseOption("Donation_Currency", "ZAR");
        ChooseOption("donationType", "One-Time");
        _driver.FindElement(By.Id("anonymousDonation")).Click();

        _driver.FindElement(By.XPath("//button[normalize-space()='Donate Now']"))
               .Click();

        _wait.Until(d => d.Url.Contains("/Donor/Donations/Confirmation"));

        var pageText = _driver.FindElement(By.TagName("body")).Text;
        StringAssert.Contains(pageText, "Thank You for Your Donation");
        StringAssert.Contains(pageText, "GOG-");
        StringAssert.Contains(pageText, "ZAR");
        StringAssert.Contains(pageText, "One-Time");
    }

    // Test 2: recurring donation in EUR, which reveals the frequency list.
    [TestMethod]
    public void RecurringDonation_MonthlyEur_ShowsConfirmation()
    {
        _driver.Navigate().GoToUrl(CreateUrl);

        _driver.FindElement(By.Id("Donation_Amount")).SendKeys("100");
        ChooseOption("Donation_Currency", "EUR");
        ChooseOption("donationType", "Recurring");

        // The frequency list is hidden until "Recurring" is chosen.
        _wait.Until(d => d.FindElement(By.Id("Donation_RecurringFrequency")).Displayed);
        ChooseOption("Donation_RecurringFrequency", "Monthly");

        _driver.FindElement(By.XPath("//button[normalize-space()='Donate Now']"))
               .Click();

        _wait.Until(d => d.Url.Contains("/Donor/Donations/Confirmation"));

        var pageText = _driver.FindElement(By.TagName("body")).Text;
        StringAssert.Contains(pageText, "Thank You for Your Donation");
        StringAssert.Contains(pageText, "EUR");
        StringAssert.Contains(pageText, "Recurring");
    }

    // Test 3 (negative path): empty amount must not create a donation.
    [TestMethod]
    public void Donation_EmptyAmount_ShowsValidationError()
    {
        _driver.Navigate().GoToUrl(CreateUrl);

        ChooseOption("Donation_Currency", "ZAR");

        _driver.FindElement(By.XPath("//button[normalize-space()='Donate Now']"))
               .Click();

        // A validation message should appear next to the Amount field.
        var message = _wait.Until(d =>
        {
            var element = d.FindElement(
                By.CssSelector("span[data-valmsg-for='Donation.Amount']"));
            return element.Text.Length > 0 ? element : null;
        });

        Assert.IsTrue(message.Text.Length > 0);
        StringAssert.Contains(_driver.Url, "/Donor/Donations/Create");
        Assert.IsFalse(_driver.Url.Contains("Confirmation"));
    }
}