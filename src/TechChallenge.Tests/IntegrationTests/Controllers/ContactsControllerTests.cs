using Xunit;
using TechChallenge.Application.Services;
using TechChallenge.Infrastructure.Repositories;
using TechChallenge.Tests.Utilities;
using TechChallenge.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Application.Models;

namespace TechChallenge.Tests.IntegrationTests.Controllers
{
    public class ContactsControllerTests
    {
        private readonly ContactsController _controller;

        public ContactsControllerTests()
        {
            var context = TestDbContextFactory.CreateInMemoryContext();
            var repository = new ContactRepository(context);
            var service = new ContactService(repository);

            _controller = new ContactsController(service);
        }

        [Fact]
        public async Task GetAllContacts_ShouldReturnOkResult()
        {
            var result = await _controller.GetAllContacts();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var contacts = Assert.IsType<List<ContactModel>>(okResult.Value);

            Assert.Equal(2, contacts.Count);
        }

        [Fact]
        public async Task CreateContact_ShouldReturnCreatedAtActionResult()
        {
            var contactDto = new ContactModel
            {
                Name = "New Contact",
                PhoneNumber = "98765432",
                Email = "newcontact@example.com",
                Ddd = "015"
            };

            var result = await _controller.CreateContact(contactDto);
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var newContact = Assert.IsType<ContactModel>(createdAtResult.Value);

            Assert.Equal("New Contact", newContact.Name);
        }
    }
}
