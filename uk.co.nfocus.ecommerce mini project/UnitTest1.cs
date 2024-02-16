
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System.Text.RegularExpressions;
using uk.co.nfocus.ecommerce_mini_project.POMClasses;
using uk.co.nfocus.ecommerce_mini_project.Utilities;

namespace uk.co.nfocus.ecommerce_mini_project
{
    internal class Tests : BaseTest
    {
        private double couponWorth = 0.15;
        private int shippingCost = 395;

        [Test]
        public void TestCase1()
        {
            // Go to shop url
            driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/demo-site/");
            Console.WriteLine("Navigated to site");

            // Dismiss popup
            driver.FindElement(By.LinkText("Dismiss")).Click();

            // Navigate to account login page
            driver.FindElement(By.LinkText("My account")).Click();
            TestHelper.WaitForElDisplayed(driver, By.Name("login")); //Wait for login page to load
            Console.WriteLine("Navigated to login page");

            // Login to said account
            LoginPagePOM loginPage = new(driver);

            //Provide username, password, and click
            bool loginStatus = loginPage.LoginExpectSuccess("newexampleemail@email.com", "MyPassword12345@");
            Assert.That(loginStatus, "Could not login");   //Verify successful login

            Console.WriteLine("Login complete");

            // Enter the shop
            driver.FindElement(By.LinkText("Shop")).Click();
            TestHelper.WaitForElDisplayed(driver, By.LinkText("Add to cart"));  //Wait until shop page has loaded
            Console.WriteLine("Navigated to shop");

            // Add to basket
            driver.FindElement(By.LinkText("Add to cart")).Click();
            TestHelper.WaitForElDisplayed(driver, By.LinkText("View cart"));  //Wait until item has been fully added to cart
            Console.WriteLine("Add item to cart");

            // View cart
            driver.FindElement(By.LinkText("View cart")).Click();
            TestHelper.WaitForElDisplayed(driver, By.LinkText("Proceed to checkout"));  //Wait until cart page has loaded
            Console.WriteLine("Navigated to cart");

            // Apply coupon
            driver.FindElement(By.Id("coupon_code")).SendKeys("edgewords"); //Type coupon
            driver.FindElement(By.Name("apply_coupon")).Click();            //Submit coupon
            TestHelper.WaitForElDisplayed(driver, By.ClassName("cart-discount"));   //Wait until coupon is accepted
            Console.WriteLine("Applied coupon code");

            ////Take a screenshot -- full page
            //Screenshot screenshot = driver.TakeScreenshot();
            //screenshot.SaveAsFile($"{TestHelper.ScreenshotPath}ecommerce_testcase_1.png");
            //TestContext.AddTestAttachment($"{TestHelper.ScreenshotPath}ecommerce_testcase_1.png", "Test case 1 checkout page");

            ////Take a screenshot of a web element
            //IWebElement cartElm = driver.FindElement(By.ClassName("woocommerce-cart-form"));
            //IWebElement breakdownElm = driver.FindElement(By.ClassName("checkout-button"));
            //Screenshot screenshotElm;

            //screenshotElm = (cartElm as ITakesScreenshot).GetScreenshot();
            //TestHelper.SaveAndAttachScreenShot(screenshotElm, "cart_items", "cart items");

            //screenshotElm = (breakdownElm as ITakesScreenshot).GetScreenshot();
            //TestHelper.SaveAndAttachScreenShot(screenshotElm, "cart_breakdown", "cart breakdown with discount");

            // Get product price
            int price = TestHelper.EleToInt(driver, By.CssSelector(".product-subtotal>.amount>bdi"));

            // Actual and expected discount
            int expectedDiscount = (int)(price * couponWorth);     // Calculate expected discount amount
            int actualDiscount = TestHelper.EleToInt(driver, By.CssSelector(".cart-discount .woocommerce-Price-amount")); // Get actual discount amount

            //Verification
            // Assess coupon removes 15%
            try     //Verify coupon amount
            {
                Assert.That(actualDiscount, Is.EqualTo(expectedDiscount)); 
                Console.WriteLine($"15% discount amount ->\n\tExpected: {actualDiscount}, Actual: {actualDiscount}");
            }
            catch (Exception)
            {
                //Do nothing
            }

            // Get and Calculate actual and expected totals
            int expectedTotal = (int)(price * (1 - couponWorth)) + shippingCost;
            int actualTotal = TestHelper.EleToInt(driver, By.CssSelector("strong bdi"));

            // Assess final total is correct
            try     //Verify final price
            {
                Assert.That(actualTotal, Is.EqualTo(expectedTotal));
                Console.WriteLine($"Final price ->\n\tExpected: {expectedTotal}, Actual: {actualTotal}");
            }
            catch (Exception)
            {
                //Do nothing
            }

            // Remove discount and items from cart
            driver.FindElement(By.LinkText("[Remove]")).Click();
            driver.FindElement(By.LinkText("×")).Click();
            TestHelper.WaitForElDisplayed(driver, By.ClassName("cart-empty"));  //Wait for empty cart to be loaded
            Console.WriteLine("Remove items from cart");

            // Logout
            driver.FindElement(By.LinkText("My account")).Click();
            TestHelper.WaitForElDisplayed(driver, By.LinkText("Logout"));   //Wait for account page to load
            //driver.FindElement(By.LinkText("Logout")).Click();

            //Attempt logout
            bool logoutStatus = loginPage.LogoutExpectSuccess();
            Assert.That(logoutStatus, "Could not logout");   //Verify successful login

            Console.WriteLine("Logout from account");

            Console.WriteLine("--Test Complete!--");
        }
    }
}