using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace GiftoftheGiverz.UITests;

[TestClass]
public class LoginTests
{
    private IWebDriver _driver = null!;
    private WebDriverWait _wait = null!;
    private const string BaseUrl = "https://localhost:7162";

    [TestInitialize]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AcceptInsecureCertificates = true;

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

    [TestMethod]
    public void EmployeeLogin_ValidCredentials_ReachesDashboard()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");

        _driver.FindElement(By.Id("Input_Email"))
               .SendKeys("employee@giftofthegiverz.co.za");
        _driver.FindElement(By.Id("Input_Password"))
               .SendKeys("Employee@123");
        _driver.FindElement(By.Id("login-submit")).Click();

        _wait.Until(d => d.Url.Contains("/Employee/Dashboard"));

        StringAssert.Contains(_driver.Url, "/Employee/Dashboard");
    }

    [TestMethod]
    public void Login_WrongPassword_ShowsErrorMessage()
    {
        _driver.Navigate().GoToUrl($"{BaseUrl}/Identity/Account/Login");

        _driver.FindElement(By.Id("Input_Email"))
               .SendKeys("employee@giftofthegiverz.co.za");
        _driver.FindElement(By.Id("Input_Password"))
               .SendKeys("WrongPassword!1");
        _driver.FindElement(By.Id("login-submit")).Click();

        var error = _wait.Until(d =>
            d.FindElement(By.CssSelector(".validation-summary-errors")));

        StringAssert.Contains(error.Text, "Invalid login attempt");
    }
}