using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Entities;
using TechChallenge.Infrastructure.Data;

namespace TechChallenge.Tests.Utilities
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TechChallengeTestDb")
                .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Contacts.AddRange(
                new Contact { Id = Guid.NewGuid(), Name = "John Doe", PhoneNumber = "12345678", Email = "johndoe@example.com", Ddd = "011" },
                new Contact { Id = Guid.NewGuid(), Name = "Jane Smith", PhoneNumber = "87654321", Email = "janesmith@example.com", Ddd = "012" }
            );
            context.SaveChanges();

            return context;
        }
    }
}
