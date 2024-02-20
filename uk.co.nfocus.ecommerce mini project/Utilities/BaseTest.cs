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

        //TO:DO > Environment variables

        [SetUp]
        protected void SetUp()
        {
            // Set up the driver instance
            //driver = new EdgeDriver();

            string browser = "firefox";

            //Instantiate a browser based on variable
            switch (browser)
            {
                case "edge":
                    driver = new EdgeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                default:
                    ChromeOptions options = new ChromeOptions();
                    options.BrowserVersion = "canary"; //stable/beta/dev/canary/num
                    driver = new ChromeDriver(options);
                    break;
            }

            //Implicit wait
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            //Do not use in production tests - has undefined behaviour.
            //Use WebDriverWaits at key synchronisation points instead
        }

        [TearDown]
        protected void TearDown()
        {
            // Quit and dispose of driver
            driver.Quit();
        }
    }
}
