using System;
using System.Linq;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure
{
    public static class DbInitializer
    {
        //public static void Initialize(ApplicationDbContext context)
        //{
        //    if (context.Movies.Any())
        //    {
        //        return; 
        //    }

        //    var Movies = new Movie[]
        //    {
        //        new Movie { Id = Guid.NewGuid(), Name = "John Doe", PhoneNumber = "12345678", Email = "johndoe@example.com", Ddd = "011" },
        //        new Movie { Id = Guid.NewGuid(), Name = "Jane Smith", PhoneNumber = "87654321", Email = "janesmith@example.com", Ddd = "012" }
        //    };

        //    context.Movies.AddRange(Movies);
        //    context.SaveChanges();
        //}
    }
}
