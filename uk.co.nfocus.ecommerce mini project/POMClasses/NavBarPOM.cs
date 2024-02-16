using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uk.co.nfocus.ecommerce_mini_project.POMClasses
{
    internal class NavBarPOM
    {
        private IWebDriver _driver;

        public NavBarPOM(IWebDriver driver)
        {
            this._driver = driver;  //Provide driver

            Assert.That(_driver.FindElement(By.LinkText("Edgewords Shop")),
                        Is.Not.Null, 
                        "Not in the edgewords shop or navbar not available");    //Verify we are on the correct website
        }

        //Locators
        private IWebElement _homeButton => _driver.FindElement(By.LinkText("Home"));
        private IWebElement _shopButton => _driver.FindElement(By.LinkText("Shop"));
        private IWebElement _cartButton => _driver.FindElement(By.LinkText("Cart"));
        private IWebElement _checkoutButton => _driver.FindElement(By.LinkText("Checkout"));
        private IWebElement _accountButton => _driver.FindElement(By.LinkText("My account"));
        private IWebElement _blogButton => _driver.FindElement(By.LinkText("Blog"));

        //Service methods

        // Go to home page
        public void GoHome()
        {
            _homeButton.Click();
        }

        // Go to shop page
        public void GoShop()
        {
            _shopButton.Click();
        }

        // Go to cart page
        public void GoCart()
        {
            _cartButton.Click();
        }

        // Go to checkout page
        public void GoCheckout()
        {
            _checkoutButton.Click();
        }

        // Go to account page
        public void GoAccount()
        {
            _accountButton.Click();
        }

        // Go to blog page
        public void GoBlog()
        {
            _blogButton.Click();
        }
    }
}
