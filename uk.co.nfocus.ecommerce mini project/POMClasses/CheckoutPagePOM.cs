using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static uk.co.nfocus.ecommerce_mini_project.Utilities.TestHelper;

namespace uk.co.nfocus.ecommerce_mini_project.POMClasses
{
    internal class CheckoutPagePOM
    {
        private IWebDriver _driver;

        public CheckoutPagePOM(IWebDriver driver)
        {
            this._driver = driver;  //Provide driver

            Assert.That(_driver.Url,
                        Does.Contain("checkout"),
                        "Not on the checkout page");   //Verify we are on the correct page
        }

        //Locators

        private IWebElement _firstNameField => _driver.FindElement(By.Id("billing_first_name"));
        //public string SetFirstName
        //{
        //    get => _firstNameField.Text;
        //    set => _firstNameField.SendKeys(value);
        //}

        private IWebElement _lastNameField => _driver.FindElement(By.Id("billing_last_name"));
        private IWebElement _countryDropDown => _driver.FindElement(By.Id("billing_country"));
        private IWebElement _streetField => _driver.FindElement(By.Id("billing_address_1"));
        private IWebElement _cityField => _driver.FindElement(By.Id("billing_city"));
        private IWebElement _postcodeField => _driver.FindElement(By.Id("billing_postcode"));
        private IWebElement _phoneNumberField => _driver.FindElement(By.Id("billing_phone"));
        private IWebElement _placeOrderButton => _driver.FindElement(By.Id("place_order"));
        //private IWebElement _placeOrderButton => _driver.FindElement(By.LinkText("Place order"));

        //Service methods

        // Set the street in the first street field
        public CheckoutPagePOM SetFirstName(string name)
        {
            _firstNameField.Clear();
            _firstNameField.SendKeys(name);
            return this;
        }

        // Set the street in the last street field
        public CheckoutPagePOM SetLastName(string name)
        {
            _lastNameField.Clear();
            _lastNameField.SendKeys(name);
            return this;
        }

        // Set the street in the street address field
        public CheckoutPagePOM SetStreetAddress(string street)
        {
            _streetField.Clear();
            _streetField.SendKeys(street);
            return this;
        }

        // Set the town or city in the city field
        public CheckoutPagePOM SetCityField(string city)
        {
            _cityField.Clear();
            _cityField.SendKeys(city);
            return this;
        }

        // Set the postcode in the postcode field
        public CheckoutPagePOM SetPostcodeField(string postcode)
        {
            _postcodeField.Clear();
            _postcodeField.SendKeys(postcode);
            return this;
        }

        // Set the phone number in the phone number field
        public CheckoutPagePOM SetPhoneNumberField(string phoneNumber)
        {
            _phoneNumberField.Clear();
            _phoneNumberField.SendKeys(phoneNumber);
            return this;
        }

        // Select country from the dropdown
        public CheckoutPagePOM SelectCounrtyDropdown(string country)
        {
            new SelectElement(_countryDropDown).SelectByText(country);
            return this;
        }

        // Place and submit the order by clicking the place order button
        public void ClickPlaceOrder()
        {
            //Console.WriteLine("Position: " + _driver.FindElement(By.Id("payment")).GetCssValue("position"));
            //new WebDriverWait(_driver, TimeSpan.FromSeconds(4)).Until(drv => drv.FindElement(By.Id("payment")).GetCssValue("position")=="relative");
            //WaitForElDisplayed(_driver, By.Id("place_order"));

            _placeOrderButton.Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(4)).Until(drv => drv.Url.Contains("order"));    //Wait for order summary page to show
        }

        //Highlevel service methods

        public void CheckoutExpectSuccess(string firstName, string lastName, string country, string street, string city, string postcode, string phoneNumber)
        {
            // Set text field information
            SetFirstName(firstName);
            SetLastName(lastName);
            SetStreetAddress(street);
            SetCityField(city);
            SetPostcodeField(postcode);
            SetPhoneNumberField(phoneNumber);

            // Select from dropdown
            SelectCounrtyDropdown(country);

            //Thread.Sleep(2000);

            //Loop over button click until it is loaded onto page
            for(int i=0; i<10; i++)
            {
                try
                {
                    //Console.WriteLine("For loop i is " + i);
                    ClickPlaceOrder();
                    break;
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }
        }
    }
}
