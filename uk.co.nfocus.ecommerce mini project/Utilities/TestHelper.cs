using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace uk.co.nfocus.ecommerce_mini_project.Utilities
{
    internal static class TestHelper
    {
        private static string _screenshotPath = @"C:\Users\JamesChurcher\OneDrive - nFocus Limited\Pictures\Screenshots\";
        public static string ScreenshotPath => _screenshotPath;

        // Explicit wait for element to be displayed
        public static void WaitForElDisplayed(IWebDriver driver, By locator)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(4));
            wait.Until(drv => drv.FindElement(locator).Displayed);
        }

        // Remove all non numerical characters from a string
        // Returns integer
        public static int StringToInt(string myString)
        {
            return int.Parse(Regex.Replace(myString, "[^0-9]", ""));
        }

        // Get only the numerical characters from the text of a located web element
        // Returns integer
        public static int EleToInt(IWebDriver driver, By locator)
        {
            var text = driver.FindElement(locator).Text;
            return StringToInt(text);
        }

        public static void SaveAndAttachScreenShot(Screenshot screenshot, string name, string description=null)
        {
            screenshot.SaveAsFile($"{TestHelper.ScreenshotPath}{name}.png");
            TestContext.AddTestAttachment($"{TestHelper.ScreenshotPath}{name}.png", description);
        }
    }
}
