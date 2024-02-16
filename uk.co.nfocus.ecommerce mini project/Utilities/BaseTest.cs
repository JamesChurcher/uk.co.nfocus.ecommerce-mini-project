using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.nfocus.ecommerce_mini_project.Utilities
{
    internal class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        protected void SetUp()
        {
            // Set up the driver instance
            driver = new EdgeDriver();

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
