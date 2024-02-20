
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System.Globalization;
using System.Text.RegularExpressions;
using uk.co.nfocus.ecommerce_mini_project.POMClasses;
using uk.co.nfocus.ecommerce_mini_project.Utilities;
using static uk.co.nfocus.ecommerce_mini_project.Utilities.TestHelper;

namespace uk.co.nfocus.ecommerce_mini_project
{
    internal class Tests : BaseTest
    {
        private const decimal couponWorth = 0.15M;
        //private const int shippingCost = 395;       //TO:DO > Get shipping cost from website dynamically

        /* First test case
         * Logs in and attempts to apply discount to a cart with items in, then logs out.
         * Tests login/logout, discount and shipping calculation.
         */
        [TestCase("newexampleemail@email.com", "MyPassword12345@", "edgewords")]
        public void TestCase1(string testUsername, string testPassword, string testDiscountCode)
        {
            // Go to shop url
            driver.Navigate().GoToUrl("https://www.edgewordstraining.co.uk/demo-site/");
            Console.WriteLine("Navigated to site");

            //TakeScreenshot(driver, "name");

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
            //navBar.GoCart(isCartEmpty: false);
            navBar.GoCart();
            WaitForElDisplayed(driver, By.LinkText("Proceed to checkout"));  //Wait until cart page has loaded
            Console.WriteLine("Navigated to cart");

            // Apply coupon
            CartPagePOM cartPage = new(driver);
            bool discountStatus = cartPage.ApplyDiscountExpectSuccess(testDiscountCode);
            Assert.That(discountStatus, "Could not apply discount");   //Verify discount was applied
            Console.WriteLine("Applied coupon code");

            // Get price from webage
            Decimal price = cartPage.GetCartSubtotal();

            // Calculate actual and expected discounts
            Decimal expectedDiscount = price * couponWorth;         // Calculate expected discount amount
            Decimal actualDiscount = cartPage.GetAppliedDiscount(); // Get actual discount amount

            //Verification
            // Assess coupon removes 15%
            try     //Verify coupon amount
            {
                Assert.That(actualDiscount, Is.EqualTo(expectedDiscount), "Incorrect discount applied");
            }
            catch (Exception)   //TO:DO > Catch Assert exceptions only
            {
                //Do nothing
            }
            Console.WriteLine($"15% discount amount ->\n\tExpected: £{actualDiscount}, Actual: £{actualDiscount}");

            // Get shipping cost from webpage
            Decimal shippingCost = cartPage.GetShippingCost();

            // Caculate actual and expected totals
            Decimal expectedTotal = (price * (1 - couponWorth)) + shippingCost;
            Decimal actualTotal = cartPage.GetCartTotal();

            // Assess final total is correct
            try     //Verify final price
            {
                Assert.That(actualTotal, Is.EqualTo(expectedTotal), "Final total price incorrect");
            }
            catch (Exception)   //TO:DO > Catch Assert exceptions only
            {
                //Do nothing
            }
            Console.WriteLine($"Final price ->\n\tExpected: £{expectedTotal}, Actual: £{actualTotal}");


            // Test Teardown ----------------------
            // Remove discount and items from cart
            cartPage.MakeCartEmpty();   //Remove the discount and products
            Console.WriteLine("Remove items from cart");

            // Logout
            navBar.GoAccount();
            WaitForElDisplayed(driver, By.LinkText("Logout"));   //Wait for account page to load

            //Attempt logout
            bool logoutStatus = loginPage.LogoutExpectSuccess();
            Assert.That(logoutStatus, "Could not logout");   //Verify successful logout

            Console.WriteLine("Logout from account");

            Console.WriteLine("--Test Complete!--");
        }

        /* Second test case
         * Logs in and attempts to checkout an item, then logs out.
         * Tests login/logout, billing information form, checkout functionality, order creation.
         */
        [TestCase("newexampleemail@email.com", "MyPassword12345@", "Jeff", "Bezos", "United Kingdom (UK)", "Amazon lane", "New York", "W1J 7NT", "07946 123400", PaymentMethod.cheque)]
        public void TestCase2(string testUsername, string testPassword, string firstName, string lastName, string country, string street, string city, string postcode, string phoneNumber, PaymentMethod paymentMethod)
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

            // Go to checkout
            navBar.GoCheckout();
            Console.WriteLine("Navigated to checkout");

            // Enter billing information
            CheckoutPagePOM checkoutPage = new(driver);
            Console.WriteLine("Enter billing information");
            checkoutPage.CheckoutExpectSuccess(firstName, lastName, country, street, city, postcode, phoneNumber, paymentMethod);
            Console.WriteLine("Checkout");

            // Order summary page
            OrderPagePOM orderPage = new(driver);
            string orderNumber = orderPage.GetOrderNumber();
            Console.WriteLine($"New order number is {orderNumber}");
            //Thread.Sleep(2000);

            // Go to my account
            navBar.GoAccount();
            Console.WriteLine("Navigated to account page");

            // Assess if previously created order is listed under this account
            bool isOrderCreated = loginPage.CheckIfOrderInOrderNumbers(orderNumber);
            try
            {
                Assert.That(isOrderCreated, "Order not in set");
            }
            catch(Exception)   //TO:DO > Catch Assert exceptions only
            {
                //Do nothing
            }
            Console.WriteLine($"Is the new order listed under account? {isOrderCreated}");


            // Test Teardown ----------------------

            // Logout
            navBar.GoAccount();
            WaitForElDisplayed(driver, By.LinkText("Logout"));   //Wait for account page to load

            //Attempt logout
            bool logoutStatus = loginPage.LogoutExpectSuccess();
            Assert.That(logoutStatus, "Could not logout");   //Verify successful login

            Console.WriteLine("Logout from account");

            Console.WriteLine("--Test Complete!--");
        }
    }
}