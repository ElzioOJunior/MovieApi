using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Models.Movie;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<MovieModel>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllAsync();
            return movies.Select(c => new MovieModel
            {
                Id = c.Id,
                Title = c.Title,
                Winner = c.Winner,
                Producers = c.Producers,
                Studios = c.Studios,
                Year = c.Year
            });
        }

        public async Task<MovieModel> CreateMovieAsync(MovieModelInsert movieModel)
        {
            var movie = new Movie
            {
                Id = new Guid(),
                Title = movieModel.Title,
                Winner = movieModel.Winner,
                Producers = movieModel.Producers,
                Studios = movieModel.Studios,
                Year = movieModel.Year
            };

            await _movieRepository.AddAsync(movie);

            return new MovieModel
            {
                Id = movie.Id,
                Title = movieModel.Title,
                Winner = movieModel.Winner,
                Producers = movieModel.Producers,
                Studios = movieModel.Studios,
                Year = movieModel.Year
            };

        }

        public async Task<MovieModel> UpdateMovieAsync(Guid id, MovieModelUpdate movieModel)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                throw new Exception("Movie not found");
            }

            movie.Id = id;
            movie.Title = movieModel.Title;
            movie.Winner = movieModel.Winner;
            movie.Producers = movieModel.Producers;
            movie.Studios = movieModel.Studios;
            movie.Year = movieModel.Year;

            await _movieRepository.UpdateAsync(movie);

            return new MovieModel
            {
                Id = id,
                Title = movieModel.Title,
                Winner = movieModel.Winner,
                Producers = movieModel.Producers,
                Studios = movieModel.Studios,
                Year = movieModel.Year
            };
        }

        public async Task<MovieModel> GetMovieByIdAsync(Guid id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            return movie == null
                ? throw new Exception("Movie not found")
                : new MovieModel
            {
                    Id = movie.Id,
                    Title = movie.Title,
                    Winner = movie.Winner,
                    Producers = movie.Producers,
                    Studios = movie.Studios,
                    Year = movie.Year
                };
        }


        public async Task<bool> DeleteMovieAsync(Guid id)
        {
            var movie = await _movieRepository.GetByIdAsync(id) ?? throw new Exception("Movie not found");
            await _movieRepository.DeleteAsync(movie);
            return true;
        }

        public async Task<PrizeIntervals> GetProducersWithPrizeIntervals()
        {
            return await _movieRepository.GetProducersWithPrizeIntervals();
         
        }
    }
}
