using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;
using TechChallenge.Domain.Entities;
using Moq;
using TechChallenge.Application.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using TechChallenge.Application.Models.Movie;

namespace TechChallenge.Tests.UnitTests.Services
{
    public class MovieServiceTests
    {
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly MovieService _movieService;

        public MovieServiceTests()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieService = new MovieService(_movieRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllMovies_ShouldReturnMovies()
        {
            var movies = new List<Movie>
                {
                    new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
                    new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" }
                };

            _movieRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(movies);

            var result = await _movieService.GetAllMoviesAsync();

            Assert.NotNull(result); 
            Assert.Equal(2, result.Count()); 
        }

    }
}
