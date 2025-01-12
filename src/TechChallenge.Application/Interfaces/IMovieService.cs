using TechChallenge.Application.Models.Movie;

namespace TechChallenge.Application.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieModel>> GetAllMoviesAsync();
        Task<MovieModel> GetMovieByIdAsync(Guid id);
        Task<MovieModel> CreateMovieAsync(MovieModelInsert movieDto);
        Task<MovieModel> UpdateMovieAsync(Guid id, MovieModelUpdate movieDto);
        Task<bool> DeleteMovieAsync(Guid id);
        Task<PrizeIntervals> GetProducersWithPrizeIntervals();

    }
}
