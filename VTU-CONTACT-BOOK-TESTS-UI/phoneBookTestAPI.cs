using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Text;

namespace VTU_CONTACT_BOOK_TESTS_UI
{
    //For all test is using AAA design
    [TestClass]
    public class phoneBookTestAPI
    {
        private HttpClient httpClient = new HttpClient();
        private string URI = "https://contactbook.evgenidimitrov0.repl.co/api";

        [TestMethod]
        [TestCategory("list all endpoints")]
        public void ListAllEndpoints()
        {
            var request = httpClient.GetAsync(URI).Result;
            var response = request.Content.ReadAsStringAsync();
            var expectedResult = "[{\"route\":\"/api\",\"method\":\"get\"},{\"route\":\"/api/contacts\",\"method\":\"get\"},{\"route\":\"/api/contacts/search/:keyword\",\"method\":\"get\"},{\"route\":\"/api/contacts/:id\",\"method\":\"get\"},{\"route\":\"/api/contacts\",\"method\":\"post\"},{\"route\":\"/api/contacts/:id\",\"method\":\"delete\"}]";

            Assert.AreEqual(200, (int)request.StatusCode);
            Assert.AreEqual(response.Result, expectedResult);
        }

        [TestMethod]
        [TestCategory("list all contacts")]
        public void ListAllContacts()
        {
            var request = httpClient.GetAsync($"{URI}/contacts").Result;
            var response = request.Content.ReadAsStringAsync();

            Assert.AreEqual(200, (int)request.StatusCode);
            Assert.IsTrue(response.Result.Contains("\"id\":3,\"firstName\":\"Anne\",\"lastName\":\"Hathaway\""));
        }

        [TestMethod]
        [TestCategory("list specified contact by ID")]
        public void ListSpecifiedContactById()
        {
            var contactId = 1;
            var request = httpClient.GetAsync($"{URI}/contacts/{contactId}").Result;
            var response = request.Content.ReadAsStringAsync();

            Assert.AreEqual(200, (int)request.StatusCode);
            Assert.IsTrue(response.Result.Contains("Elon"));
        }

        [TestMethod]
        [TestCategory("search by keyword")]
        public void SearchByKeyword()
        {
            var keyword = "dimitar";
            var request = httpClient.GetAsync($"{URI}/contacts/search/{keyword}").Result;
            var response = request.Content.ReadAsStringAsync();

            Assert.AreEqual(200, (int)request.StatusCode);
            Assert.IsTrue(response.Result.Contains("\"id\":2,\"firstName\":\"Dimitar\",\"lastName\":\"Berbatov\""));
        }

        [TestMethod]
        [TestCategory("search by invalid keyword")]
        public void SearchByInvalidKeyword()
        {
            Random rnd = new Random();
            int random = rnd.Next();
            var keyword = $"missing{random}";
            var request = httpClient.GetAsync($"{URI}/contacts/search/{keyword}").Result;
            var response = request.Content.ReadAsStringAsync();

            Assert.AreEqual(200, (int)request.StatusCode);
            Assert.AreEqual("[]", response.Result);
        }

        [TestMethod]
        [TestCategory("create new contact")]
        public void CreateNewContact()
        {
            var content = new StringContent("{\"firstName\":\"Stanislav\", \"lastName\":\"Temelkov\", \"email\":\"stanislav.temelkov@hotmail.com\", \"phone\":\"0416484648\", \"comments\":\"Stanislav Temelkov contacts\"}", Encoding.UTF8, "application/json");
            var request = httpClient.PostAsync($"{URI}/contacts", content).Result;

            Assert.AreEqual("Created", request.ReasonPhrase);
            Assert.AreEqual(200, (int)request.StatusCode);
        }

        [TestMethod]
        [TestCategory("create new invalid contact")]
        public void CreateNewInvalidContact()
        {
            var content = new StringContent("{\"firstName\":\"0416484648\", \"lastName\":\"stanislav.temelkov@hotmail.com\", \"email\":\"stanislav\", \"phone\":\"0416484648\", \"comments\":\"Stanislav Temelkov contacts\"}", Encoding.UTF8, "application/json");
            var request = httpClient.PostAsync($"{URI}/contacts", content).Result;

            Assert.AreNotEqual(200, (int)request.StatusCode);
        }

        [TestMethod]
        [TestCategory("create contact, delete contact by ID, search for contact")]
        public void DeleteContactById()
        {
            var contentCreateContact = new StringContent("{\"firstName\":\"Stanislav\", \"lastName\":\"Temelkov\", \"email\":\"stanislav.temelkov@hotmail.com\", \"phone\":\"0416484648\", \"comments\":\"Stanislav Temelkov contacts\"}", Encoding.UTF8, "application/json");
            var requestCreateContact = httpClient.PostAsync($"{URI}/contacts", contentCreateContact).Result;

            var contactId = "21";
            var response = httpClient.DeleteAsync($"{URI}/contacts/{contactId}").Result;

            var requestContacts = httpClient.GetAsync($"{URI}/contacts").Result;
            var responseContacts = requestContacts.Content.ReadAsStringAsync();

            Assert.AreEqual("Created", requestCreateContact.ReasonPhrase);
            Assert.IsFalse(responseContacts.Result.Contains($"\"id\":{contactId}"));
            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.AreEqual(200, (int)requestContacts.StatusCode);
        }
    }
}