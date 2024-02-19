using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.nfocus.ecommerce_mini_project.Utilities;
using static uk.co.nfocus.ecommerce_mini_project.Utilities.TestHelper;

namespace uk.co.nfocus.ecommerce_mini_project.POMClasses
{
    internal class CartPagePOM
    {
        private IWebDriver _driver;

        public CartPagePOM(IWebDriver driver)
        {
            this._driver = driver;  //Provide driver

            Assert.That(_driver.Url,
                        Does.Contain("cart"),
                        "Not in the cart / on cart page");   //Verify we are on the correct page
        }

        //Locators
        //private IWebElement _discountCodeField => _driver.FindElement(By.Id("coupon_code"));
        private IWebElement _discountCodeField
        {
            get
            {
                WaitForElDisplayed(_driver, By.Id("coupon_code"));
                return _driver.FindElement(By.Id("coupon_code"));
            }
        }
        private IWebElement _applyDiscountButton => _driver.FindElement(By.Name("apply_coupon"));
        private IWebElement _removeFromCartButton => _driver.FindElement(By.LinkText("×"));
        private IWebElement _removeDiscountButton => _driver.FindElement(By.LinkText("[Remove]"));

        //private IWebElement _cartDiscountLabel => _driver.FindElement(By.CssSelector(".cart-discount .amount"));    //TO:DO > Apply wait
        private IWebElement _cartDiscountLabel
        {
            get
            {
                WaitForElDisplayed(_driver, By.CssSelector(".cart-discount .amount"));
                return _driver.FindElement(By.CssSelector(".cart-discount .amount"));
            }
        }
        private IWebElement _cartTotalLabel => _driver.FindElement(By.CssSelector(".order-total bdi"));
        private IWebElement _cartSubtotalLabel => _driver.FindElement(By.CssSelector(".cart-subtotal bdi"));

        //Service methods

        //Enter discount code
        public CartPagePOM SetDiscountCode(string code)
        {
            _discountCodeField.SendKeys(code);
            return this;
        }

        public void SubmitDiscountForm()
        {
            _applyDiscountButton.Click();
        }

        public void ClickRemoveDiscountButton()
        {
            _removeDiscountButton.Click();
        }
        
        public void ClickRemoveItemButton()
        {
            _removeFromCartButton.Click();
        }

        public string GetAppliedDiscount()
        {
            return _cartDiscountLabel.Text;
        }

        public string GetCartSubtotal()
        {
            return _cartSubtotalLabel.Text;
        }

        public string GetCartTotal()
        {
            return _cartTotalLabel.Text;
        }

        //Highlevel service methods

        public bool ApplyDiscountExpectSuccess(string discountCode)
        {
            SetDiscountCode(discountCode);
            SubmitDiscountForm();

            try
            {
                WaitForElDisplayed(_driver, By.CssSelector(".cart-discount .amount"));  //Wait until login has completed
                return true;    //Coupon applied
            }
            catch (NoSuchElementException e)
            {
                return false;   //Coupon rejected
            }
        }

        //Remove the discount and items from cart
        public void MakeCartEmpty()
        {
            ClickRemoveDiscountButton();
            //new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(drv => (drv.FindElements(By.LinkText("[Remove]")).Count==0));    //Wait until discount is no longer applied
            //Console.WriteLine("Is remove item button clickable? " + _removeFromCartButton.Enabled);

            ClickRemoveItemButton();
            //new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(drv => !_removeFromCartButton.Displayed);    //Wait until item is no longer applied

            WaitForElDisplayed(_driver, By.ClassName("cart-empty"));  //Wait for empty cart to be loaded
        }
    }
}
