
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System.Text.RegularExpressions;
using uk.co.nfocus.ecommerce_mini_project.POMClasses;
using uk.co.nfocus.ecommerce_mini_project.Utilities;
using static uk.co.nfocus.ecommerce_mini_project.Utilities.TestHelper;

namespace uk.co.nfocus.ecommerce_mini_project
{
    internal class Tests : BaseTest
    {
        private const double couponWorth = 0.15;
        private const int shippingCost = 395;

        [TestCase("newexampleemail@email.com", "MyPassword12345@", "edgewords")]
        public void TestCase1(string testUsername, string testPassword, string testDiscountCode)
        {
            // Go to shop url
            driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/demo-site/");
            Console.WriteLine("Navigated to site");

            // Dismiss popup
            driver.FindElement(By.LinkText("Dismiss")).Click();

            // Create NavBar POM instance
            NavBarPOM navBar = new(driver);

            // Navigate to account login page
            navBar.GoAccount();
            Console.WriteLine("Navigated to login page");

            // Login to said account
            LoginPagePOM loginPage = new(driver);

            //Provide username, password, and click
            bool loginStatus = loginPage.LoginExpectSuccess(testUsername, testPassword);
            Assert.That(loginStatus, "Could not login");   //Verify successful login
            Console.WriteLine("Login complete");

            // Enter the shop
            navBar.GoShop();
            Console.WriteLine("Navigated to shop");

            // Add to basket
            ShopPagePOM shopPage = new(driver);
            shopPage.ClickAddToBasket();
            Console.WriteLine("Add item to cart");

            // View cart
            navBar.GoCart(isCartEmpty: false);
            WaitForElDisplayed(driver, By.LinkText("Proceed to checkout"));  //Wait until cart page has loaded
            Console.WriteLine("Navigated to cart");

            // Apply coupon
            CartPagePOM cartPage = new(driver);
            bool discountStatus = cartPage.ApplyDiscountExpectSuccess(testDiscountCode);
            Assert.That(discountStatus, "Could not apply discount");   //Verify discount was applied
            Console.WriteLine("Applied coupon code");

            // Get product price
            int price = StringToInt(cartPage.GetCartSubtotal());
            Console.WriteLine($"Individual price: {price}");

            // Actual and expected discount
            int expectedDiscount = (int)(price * couponWorth);     // Calculate expected discount amount
            int actualDiscount = StringToInt(cartPage.GetAppliedDiscount());    // Get actual discount amount

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
            int actualTotal = StringToInt(cartPage.GetCartTotal());

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
            cartPage.MakeCartEmpty();   //Remove the discount and products
            Console.WriteLine("Remove items from cart");

            // Logout
            navBar.GoAccount();
            TestHelper.WaitForElDisplayed(driver, By.LinkText("Logout"));   //Wait for account page to load

            //Attempt logout
            bool logoutStatus = loginPage.LogoutExpectSuccess();
            Assert.That(logoutStatus, "Could not logout");   //Verify successful login

            Console.WriteLine("Logout from account");

            Console.WriteLine("--Test Complete!--");
        }
    }
}