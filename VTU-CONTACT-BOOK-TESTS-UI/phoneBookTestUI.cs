using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace VTU_CONTACT_BOOK_TESTS_UI
{
    //For all test is using AAA design
    [TestClass]
    public class phoneBookTestUI
    {
        private IWebDriver driver;

        private void StartUp()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://contactbook.evgenidimitrov0.repl.co");
        }

        [TestMethod]
        [TestCategory("try to add new contact")]
        public void addNewContact()
        {
            StartUp();

            var addcontactButton = driver.FindElement(By.XPath("/html/body/main/div/a[2]"));
            addcontactButton.Click();
            var firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.SendKeys("Stanislav");
            var lastNameField = driver.FindElement(By.Id("lastName"));
            lastNameField.SendKeys("Temelkov");
            var emailField = driver.FindElement(By.Id("email"));
            var email = "stanislav.temelkov@hotmail.com";
            emailField.SendKeys(email);
            var phoneField = driver.FindElement(By.Id("phone"));
            phoneField.SendKeys("0416484648");
            var commentsField = driver.FindElement(By.Id("comments"));
            commentsField.SendKeys("Stanislav Temelkov contacts");
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();
            driver.Navigate().Back();

            Assert.IsTrue(driver.FindElement(By.Id("firstName")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("lastName")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("email")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("phone")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("comments")).Displayed);
            Assert.IsTrue(driver.FindElement(By.Id("create")).Displayed);

            ShutDown();
        }

        [TestMethod]
        [TestCategory("try to add new invalid contact")]
        public void addNewInvalidContact()
        {
            StartUp();

            var addcontactButton = driver.FindElement(By.XPath("/html/body/main/div/a[2]"));
            addcontactButton.Click();
            var firstNameField = driver.FindElement(By.Id("firstName"));
            firstNameField.SendKeys("0416484648");
            var lastNameField = driver.FindElement(By.Id("lastName"));
            lastNameField.SendKeys("stanislav.temelkov@hotmail.com");
            var emailField = driver.FindElement(By.Id("email"));
            var email = "stanislav";
            emailField.SendKeys(email);
            var phoneField = driver.FindElement(By.Id("phone"));
            phoneField.SendKeys("0416484648");
            var commentsField = driver.FindElement(By.Id("comments"));
            commentsField.SendKeys("Stanislav Temelkov contacts");
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            Assert.IsTrue(driver.FindElement(By.XPath("/html/body/main/div")).Displayed);

            ShutDown();
        }

        [TestMethod]
        [TestCategory("List all contacts")]
        public void viewAllContacts()
        {
            StartUp();

            var contactsButton = driver.FindElement(By.XPath("/html/body/aside/ul/li[2]/a"));
            contactsButton.Click();
            var firstContact = driver.FindElement(By.XPath("//*[@id=\"contact1\"]/tbody/tr[1]/td"));

            Assert.IsTrue(firstContact.Text == "Elon");

            ShutDown();
        }

        [TestMethod]
        [TestCategory("search contact by keyword")]
        public void searchContactByKeyword()
        {
            StartUp();

            var searchButton = driver.FindElement(By.XPath("/html/body/aside/ul/li[4]/a"));
            searchButton.Click();
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("dimitar");
            var findButton = driver.FindElement(By.Id("search"));
            findButton.Click();
            var resultFirstName = driver.FindElement(By.XPath("//*[@id=\"contact2\"]/tbody/tr[1]/td"));
            var resultLastName = driver.FindElement(By.XPath("//*[@id=\"contact2\"]/tbody/tr[2]/td"));

            Assert.AreEqual("1 contacts found.", driver.FindElement(By.Id("searchResult")).Text);
            Assert.AreEqual("Dimitar", resultFirstName.Text);
            Assert.AreEqual("Berbatov", resultLastName.Text);

            ShutDown();
        }

        [TestMethod]
        [TestCategory("search contact by keyword")]
        public void searchContactByInvalidKeyword()
        {
            StartUp();

            var searchButton = driver.FindElement(By.XPath("/html/body/aside/ul/li[4]/a"));
            searchButton.Click();
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("Invalid key word 123");
            var findButton = driver.FindElement(By.Id("search"));
            findButton.Click();

            Assert.AreEqual("No contacts found.", driver.FindElement(By.Id("searchResult")).Text);

            ShutDown();
        }

        private void ShutDown()
        {
            driver.Quit();
        }
    }
}
