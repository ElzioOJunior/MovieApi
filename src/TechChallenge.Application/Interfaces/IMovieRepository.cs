using TechChallenge.Application.Models.Movie;
using TechChallenge.Application.Services;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync();        
        Task<Movie> GetByIdAsync(Guid id);             
        Task AddAsync(Movie movie);      
        Task AddRangeAsync(List<Movie> movies);
        Task UpdateAsync(Movie movie);               
        Task DeleteAsync(Movie movie);
        Task<PrizeIntervals> GetProducersWithPrizeIntervals();
    }
}
