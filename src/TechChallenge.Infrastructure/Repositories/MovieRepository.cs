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

            var result = new PrizeIntervals();
            var allIntervals = new List<MovieInterval>();

            foreach (var movie in moviesWithAwards)
            {
                var producers = movie.Producers.Split(',').Select(p => p.Trim()).ToList();

                foreach (var producer in producers)
                {
                    var producerGroup = moviesWithAwards
                        .Where(m => m.Producers.Contains(producer) && m.Winner == "yes")
                        .OrderBy(m => m.Year)
                        .ToList();

                    if (producerGroup.Count() > 1)
                    {
                        for (int i = 0; i < producerGroup.Count() - 1; i++)
                        {
                            var previousMovie = producerGroup[i];
                            var followingMovie = producerGroup[i + 1];

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
                }
            }

            if (allIntervals.Any())
            {
                var uniqueIntervals = allIntervals
                    .GroupBy(i => new { i.Producer, i.Interval, i.PreviousWin, i.FollowingWin })
                    .Select(group => group.First())
                    .ToList();

                var minInterval = uniqueIntervals.OrderBy(i => i.Interval).First().Interval;
                result.Min.AddRange(uniqueIntervals.Where(i => i.Interval == minInterval));

                var maxInterval = uniqueIntervals.OrderByDescending(i => i.Interval).First().Interval;
                result.Max.AddRange(uniqueIntervals.Where(i => i.Interval == maxInterval));
            }

            return result;
        }


    }
}


