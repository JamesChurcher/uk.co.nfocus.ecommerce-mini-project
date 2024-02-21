using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static uk.co.nfocus.ecommerce_mini_project.Utilities.TestHelper;

namespace uk.co.nfocus.ecommerce_mini_project.Utilities
{
    internal class BaseTest
    {
        //Holds the driver
        protected IWebDriver driver;

        [SetUp]
        protected void SetUp()
        {
            // Get environmet variables
            string browser = Environment.GetEnvironmentVariable("BROWSER");
            Console.WriteLine($"Browser is set to: {browser}");

            //string runsettings = TestContext.Parameters["WebAppUrl"];
            //Console.WriteLine("The runsettings param was " + runsettings);

            // Set up the driver instance
            //driver = new EdgeDriver();

            //string browser = "edge";

            if (browser == null)
            {
                browser = "edge";
                Console.WriteLine("BROWSER env not set: Setting default to edge");
            }

            //Instantiate a browser based on variable
            switch (browser)
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.BrowserVersion = "canary"; //stable/beta/dev/canary/num
                    driver = new ChromeDriver(options);
                    break;
                default:
                    driver = new EdgeDriver();
                    break;
            }

            //Implicit wait
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            //Do not use in production tests - has undefined behaviour.
            //Use WebDriverWaits at key synchronisation points instead

            // Go to shop url
            driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/demo-site/");
            Console.WriteLine("Navigated to site");

            // Dismiss popup
            driver.FindElement(By.LinkText("Dismiss")).Click();
        }

        [TearDown]
        protected void TearDown()
        {
            // Quit and dispose of driver
            driver.Quit();
        }
    }
}
