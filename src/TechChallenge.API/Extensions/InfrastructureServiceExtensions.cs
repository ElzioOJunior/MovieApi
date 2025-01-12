using Microsoft.EntityFrameworkCore;
using TechChallenge.Application.Interfaces;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Infrastructure.Repositories;

namespace TechChallenge.Api.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TechChallengeInMemoryDb"));


            services.AddScoped<IMovieRepository, MovieRepository>();

            return services;
        }
    }
}
