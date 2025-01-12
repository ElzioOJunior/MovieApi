using Microsoft.EntityFrameworkCore;
using System;
using TechChallenge.Domain.Entities;
using TechChallenge.Infrastructure.Data;

namespace TechChallenge.Tests.Utilities
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            var context = new ApplicationDbContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Movies.AddRange(
               new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
               new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" },
               new Movie { Id = Guid.NewGuid(), Year = "1982", Title = "Inchon", Studios = "MGM", Producers = "Mitsuharu Ishii", Winner = "yes" },
               new Movie { Id = Guid.NewGuid(), Year = "1984", Title = "Bolero", Studios = "Cannon Films", Producers = "Bo Derek", Winner = "yes" },
               new Movie { Id = Guid.NewGuid(), Year = "1990", Title = "Ghosts Can't Do It", Studios = "Triumph Releasing", Producers = "Bo Derek", Winner = "yes" }
           );


            context.SaveChanges();

            return context;
        }
    }
}
