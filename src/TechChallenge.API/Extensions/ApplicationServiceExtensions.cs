using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;

namespace TechChallenge.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IMovieService, MovieService>();

            return services;
        }
    }
}
