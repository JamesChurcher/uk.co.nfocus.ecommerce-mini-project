using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        //private IWebElement _removeFromCartButton => _driver.FindElement(By.LinkText("×"));     //TODO > Maybe change to be more robust
        private IWebElement _removeFromCartButton => _driver.FindElement(By.ClassName("remove"));
        private IWebElement _removeDiscountButton => _driver.FindElement(By.LinkText("[Remove]"));

        //private IWebElement _cartDiscountLabel => _driver.FindElement(By.CssSelector(".cart-discount .amount"));    //TODO > Apply wait
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
        private IWebElement _cartShippingCostLabel => _driver.FindElement(By.CssSelector(".shipping bdi"));

        //Service methods

        //Enter discount code
        public CartPagePOM SetDiscountCode(string code)
        {
            _discountCodeField.Clear();
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

        // Returns decimal
        public decimal GetAppliedDiscount()
        {
            return StringToDecimal(_cartDiscountLabel.Text);
        }

        public decimal GetCartSubtotal()
        {
            return StringToDecimal(_cartSubtotalLabel.Text);
        }

        public decimal GetCartTotal()
        {
            return StringToDecimal(_cartTotalLabel.Text);
        }

        public decimal GetShippingCost()
        {
            return StringToDecimal(_cartShippingCostLabel.Text);
        }

        //Highlevel service methods

        //Applied the given discount code to the current cart
        //  Params  -> discountCode: The discount code to apply
        //  Returns -> (bool) if discount code was applied successfully
        public bool ApplyDiscountExpectSuccess(string discountCode)
        {
            SetDiscountCode(discountCode);
            SubmitDiscountForm();

            try
            {
                //WaitForElDisplayed(_driver, By.CssSelector(".cart-discount .amount"));  //Wait until discount has been applied
                WaitForElDisplayed(_driver, By.LinkText("[Remove]"));  //Wait until discount has been applied
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
            //Console.WriteLine("Is remove item button clickable? " + _removeFromCartButton.Enabled);

            //Wait for banner confirming discount removal to load
            //new WebDriverWait(_driver, TimeSpan.FromSeconds(4)).Until(drv => drv.FindElement(By.ClassName("woocommerce-message")).Text.Contains("Coupon has been removed."));

            new WebDriverWait(_driver, TimeSpan.FromSeconds(4)).Until(drv => (drv.FindElements(By.LinkText("[Remove]")).Count == 0));    //Wait until discount is no longer applied

            ClickRemoveItemButton();

            //for (int i = 0; i < 10; i++)
            //{
            //    try
            //    {
            //        Console.WriteLine("For loop i is " + i);
            //        Thread.Sleep(1000);
            //        ClickRemoveItemButton();
            //        break;
            //    }
            //    catch (Exception)
            //    {
            //        Thread.Sleep(100);
            //        Console.WriteLine("Caught exception " + i);
            //        //Do nothing
            //    }
            //}

            //new WebDriverWait(_driver, TimeSpan.FromSeconds(3)).Until(drv => !_removeFromCartButton.Displayed);    //Wait until item is no longer applied

            WaitForElDisplayed(_driver, By.ClassName("cart-empty"));  //Wait for empty cart to be loaded
        }
    }
}
