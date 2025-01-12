using Xunit;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Infrastructure.Repositories;
using TechChallenge.Domain.Entities;
using TechChallenge.Tests.Utilities;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace TechChallenge.Tests.IntegrationTests.Repositories
{
    public class MovieRepositoryTests
    {
        private readonly ApplicationDbContext _context;
        private readonly MovieRepository _repository;

        public MovieRepositoryTests()
        {
            _context = TestDbContextFactory.CreateInMemoryContext();
            _repository = new MovieRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllMovies()
        {

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Preparando os dados (cinco filmes)
            _context.Movies.AddRange(
                new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1982", Title = "Inchon", Studios = "MGM", Producers = "Mitsuharu Ishii", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1984", Title = "Bolero", Studios = "Cannon Films", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1990", Title = "Ghosts Can't Do It", Studios = "Triumph Releasing", Producers = "Bo Derek", Winner = "yes" }
            );
            await _context.SaveChangesAsync();

            var movies = await _repository.GetAllAsync();

            // Verificando se o número de filmes no banco é o esperado
            Assert.Equal(5, movies.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddMovie()
        {
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Year = "2025",
                Title = "Test Movie",
                Studios = "Test Studios",
                Producers = "Test Producer",
                Winner = "no"
            };

            await _repository.AddAsync(movie);
            var result = await _repository.GetByIdAsync(movie.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal("Test Studios", result.Studios);
            Assert.Equal("Test Producer", result.Producers);
            Assert.Equal("no", result.Winner);
        }

        [Fact]
        public async Task GetProducersWithPrizeIntervals_ShouldReturnCorrectPrizeIntervals()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Movies.AddRange(
                new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1982", Title = "Inchon", Studios = "MGM", Producers = "Mitsuharu Ishii", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1984", Title = "Bolero", Studios = "Cannon Films", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1990", Title = "Ghosts Can't Do It", Studios = "Triumph Releasing", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1992", Title = "Test Movie", Studios = "Test Studios", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1983", Title = "Test Movie 2", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1985", Title = "Test Movie 3", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" }
            );
            await _context.SaveChangesAsync();

            var prizeIntervals = await _repository.GetProducersWithPrizeIntervals();

            Assert.NotNull(prizeIntervals);

            Assert.NotEmpty(prizeIntervals.Max);
            Assert.NotEmpty(prizeIntervals.Min); 

            var maxIntervalProducer = prizeIntervals.Max.FirstOrDefault(p => p.Producer == "Bo Derek");
            Assert.NotNull(maxIntervalProducer);
            Assert.Equal(6, maxIntervalProducer.Interval);  

            var minIntervalProducer = prizeIntervals.Min.FirstOrDefault(p => p.Producer == "Bo Derek");
            Assert.NotNull(minIntervalProducer);
            Assert.Equal(2, minIntervalProducer.Interval); 
        }

        [Fact]
        public async Task GetProducersWithPrizeIntervals_ShouldReturnProducerWithMaxInterval()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Movies.AddRange(
                new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1982", Title = "Inchon", Studios = "MGM", Producers = "Mitsuharu Ishii", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1984", Title = "Bolero", Studios = "Cannon Films", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1990", Title = "Ghosts Can't Do It", Studios = "Triumph Releasing", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1992", Title = "Test Movie", Studios = "Test Studios", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1983", Title = "Test Movie 2", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1985", Title = "Test Movie 3", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" }
            );

            await _context.SaveChangesAsync();

            var prizeIntervals = await _repository.GetProducersWithPrizeIntervals();

            Assert.NotNull(prizeIntervals);
            var maxIntervalProducer = prizeIntervals.Max.FirstOrDefault(p => p.Producer == "Bo Derek");
            Assert.NotNull(maxIntervalProducer);
            Assert.Equal(6, maxIntervalProducer.Interval); 
        }

        [Fact]
        public async Task GetProducersWithPrizeIntervals_ShouldReturnProducerWithMinInterval()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Movies.AddRange(
                new Movie { Id = Guid.NewGuid(), Year = "1980", Title = "Can't Stop the Music", Studios = "Associated Film Distribution", Producers = "Allan Carr", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1981", Title = "Mommie Dearest", Studios = "Paramount Pictures", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1982", Title = "Inchon", Studios = "MGM", Producers = "Mitsuharu Ishii", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1984", Title = "Bolero", Studios = "Cannon Films", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1990", Title = "Ghosts Can't Do It", Studios = "Triumph Releasing", Producers = "Bo Derek", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1992", Title = "Test Movie", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1983", Title = "Test Movie 2", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" },
                new Movie { Id = Guid.NewGuid(), Year = "1985", Title = "Test Movie 3", Studios = "Test Studios", Producers = "Frank Yablans", Winner = "yes" }
            );
            await _context.SaveChangesAsync();

            var prizeIntervals = await _repository.GetProducersWithPrizeIntervals();

            Assert.NotNull(prizeIntervals);
            var minIntervalProducer = prizeIntervals.Min.FirstOrDefault(p => p.Producer == "Frank Yablans");
            Assert.NotNull(minIntervalProducer);
            Assert.Equal(1, minIntervalProducer.Interval); 
        }
    }
}