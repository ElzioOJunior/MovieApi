using Xunit;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Infrastructure.Repositories;
using TechChallenge.Domain.Entities;
using TechChallenge.Tests.Utilities;

namespace TechChallenge.Tests.IntegrationTests.Repositories
{
    public class ContactRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ContactRepository _repository;

        public ContactRepositoryTests()
        {
            _context = TestDbContextFactory.CreateInMemoryContext();
            _repository = new ContactRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllContacts()
        {
            var contacts = await _repository.GetAllAsync();
            Assert.Equal(2, contacts.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddContact()
        {
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Name = "Test Contact",
                PhoneNumber = "98765432",
                Email = "test@example.com",
                Ddd = "013"
            };

            await _repository.AddAsync(contact);
            var result = await _repository.GetByIdAsync(contact.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Contact", result.Name);
        }
    }
}
