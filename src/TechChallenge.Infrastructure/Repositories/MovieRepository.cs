using Microsoft.EntityFrameworkCore;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Models.Movie;
using TechChallenge.Domain.Entities;
using TechChallenge.Infrastructure.Data;

namespace TechChallenge.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Movie>> GetAllAsync() => await _context.Movies.ToListAsync();

        public async Task<Movie> GetByIdAsync(Guid id) => await _context.Movies.FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<Movie> movies)
        {
            await _context.Movies.AddRangeAsync(movies);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<PrizeIntervals> GetProducersWithPrizeIntervals()
        {

            var moviesWithAwards = await _context.Movies
                .Where(m => m.Winner == "yes")
                .OrderBy(m => m.Producers)
                .ThenBy(m => m.Year)
                .ToListAsync();

            var producersGroups = moviesWithAwards
                .GroupBy(m => m.Producers)
                .Where(group => group.Count() > 1)  
                .ToList();

            var result = new PrizeIntervals();
            var allIntervals = new List<MovieInterval>();

            foreach (var producerGroup in producersGroups)
            {
                var producer = producerGroup.Key;
                var sortedMovies = producerGroup.OrderBy(m => m.Year).ToList();

                for (int i = 0; i < sortedMovies.Count - 1; i++)
                {
                    var previousMovie = sortedMovies[i];
                    var followingMovie = sortedMovies[i + 1];

                    var interval = new MovieInterval
                    {
                        Producer = producer,
                        PreviousWin = Convert.ToInt16(previousMovie.Year),
                        FollowingWin = Convert.ToInt16(followingMovie.Year),
                        Interval = Convert.ToInt16(followingMovie.Year) - Convert.ToInt16(previousMovie.Year)
                    };

                    allIntervals.Add(interval);
                }
            }

            if (allIntervals.Any())
            {
                var minInterval = allIntervals.OrderBy(i => i.Interval).First().Interval;

                result.Min.AddRange(allIntervals.Where(i => i.Interval == minInterval));

                var maxInterval = allIntervals.OrderByDescending(i => i.Interval).First().Interval;

                result.Max.AddRange(allIntervals.Where(i => i.Interval == maxInterval));
            }

            return result;
        }

    }
}


