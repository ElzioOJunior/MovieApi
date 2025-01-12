using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;
using TechChallenge.Domain.Entities;
using Moq;
using TechChallenge.Application.Models;
using Xunit;

namespace TechChallenge.Tests.UnitTests.Services
{
    public class ContactServiceTests
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _contactService = new ContactService(_contactRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllContacts_ShouldReturnContacts()
        {
            // Arrange
            var contacts = new List<ContactModel>
            {
                new ContactModel { Id = Guid.NewGuid(), Name = "John Doe", PhoneNumber = "12345678", Email = "johndoe@example.com", Ddd = "011" }
            };
            _contactRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contacts.Select(c => new Contact
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Ddd = c.Ddd
            }));

            // Act
            var result = await _contactService.GetAllContactsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
