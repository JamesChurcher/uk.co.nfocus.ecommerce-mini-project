using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uk.co.nfocus.ecommerce_mini_project.Utilities;

namespace uk.co.nfocus.ecommerce_mini_project.POMClasses
{
    internal class LoginPagePOM
    {
        private IWebDriver _driver;

        public LoginPagePOM(IWebDriver driver)
        {
            this._driver = driver;  //Provide driver

            Assert.That(_driver.Url, 
                        Does.Contain("my-account"), 
                        "Not on the account page");   //Verify we are on the correct page
        }

        //Locators
        private IWebElement _usernameField => _driver.FindElement(By.Id("username"));
        private IWebElement _passwordField => _driver.FindElement(By.Id("password"));
        private IWebElement _submitFormButton => _driver.FindElement(By.Name("login"));     //TO:DO > Add waits
        private IWebElement _logoutButton => _driver.FindElement(By.LinkText("Logout"));    //TO:DO > Add waits

        //Service methods

        //Set username in the username field
        public LoginPagePOM SetUsername(string username)
        {
            _usernameField.SendKeys(username);
            return this;
        }

        //Set password in the password field
        public LoginPagePOM SetPassword(string password)
        {
            _passwordField.SendKeys(password);
            return this;
        }

        //Login by clicking the "login" button
        public void SubmitLoginForm()
        {
            _submitFormButton.Click();
        }

        //Logout by clicking the "logout" button
        public void ClickLogout()
        {
            _logoutButton.Click();
        }

        //Higher level helpers

        //Login to account by providing username, password, and submitting form
        //  Params  -> username: Username, password: Password
        //  Returns -> (bool) if login was successful status
        public bool LoginExpectSuccess(string username, string password)
        {
            SetUsername(username);
            SetPassword(password);
            SubmitLoginForm();

            try
            {
                TestHelper.WaitForElDisplayed(_driver, By.LinkText("Logout"));  //Wait until login has completed
                //var logoutButton = _logoutButton;
                return true;    //Login success
            }
            catch (NoSuchElementException e)
            {
                return false;   //Failed login
            }
        }

        public bool LogoutExpectSuccess()
        {
            ClickLogout();

            try
            {
                TestHelper.WaitForElDisplayed(_driver, By.Name("login"));  //Wait until logout has completed
                //var loginButton = _submitFormButton;
                return true;    //Logout success
            }
            catch (NoSuchElementException e)
            {
                return false;   //Failed logout
            }
        }
    }
}
