using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Services;
using TechChallenge.Application.Models.Movie;

namespace TechChallenge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movies
        [HttpGet]
        [SwaggerOperation(Summary = "Get all Movies", Description = "Returns a list of all Movies.")]
        [SwaggerResponse(200, "List of Movies retrieved successfully.", typeof(IEnumerable<MovieModel>))]
        public async Task<ActionResult<IEnumerable<MovieModel>>> GetAllMovies()
        {
            var Movies = await _movieService.GetAllMoviesAsync();
            return Ok(Movies);
        }

        // GET: api/Movies/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get Movie by ID", Description = "Retrieves a Movie by its unique ID.")]
        [SwaggerResponse(200, "Movie retrieved successfully.", typeof(MovieModel))]
        [SwaggerResponse(404, "Movie not found.")]
        public async Task<ActionResult<MovieModel>> GetMovieById(Guid id)
        {
            try
            {
                var Movie = await _movieService.GetMovieByIdAsync(id);
                return Ok(Movie);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // GET: api/Movies/producers/prize-intervals
        [HttpGet("producers/prize-intervals")]
        [SwaggerOperation(Summary = "Get Producers with Prize Intervals", Description = "Returns the producer with the maximum and minimum prize intervals.")]
        [SwaggerResponse(200, "Producer intervals retrieved successfully.", typeof(PrizeIntervals))]
        [SwaggerResponse(404, "No data found.")]
        public async Task<ActionResult<PrizeIntervals>> GetProducersWithPrizeIntervals()
        {
            try
            {
                var result = await _movieService.GetProducersWithPrizeIntervals();

                if (result.Min.Count == 0 && result.Max.Count == 0)
                {
                    return NotFound(new { message = "No data found for prize intervals." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error." });
            }
        }


        // POST: api/Movies
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Movie", Description = "Creates a new Movie with the provided details.")]
        [SwaggerResponse(201, "Movie created successfully.", typeof(MovieModelInsert))]
        [SwaggerResponse(400, "Invalid input data.")]
        public async Task<ActionResult<MovieModel>> CreateMovie([FromBody] MovieModelInsert movieDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var result = await _movieService.CreateMovieAsync(movieDto);
            return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, result);
        }

        // PUT: api/Movies/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update Movie", Description = "Updates an existing Movie's details by its ID.")]
        [SwaggerResponse(200, "Movie updated successfully.", typeof(MovieModel))]
        [SwaggerResponse(400, "Invalid input data.")]
        [SwaggerResponse(404, "Movie not found.")]
        public async Task<ActionResult<MovieModel>> UpdateMovie(Guid id, [FromBody] MovieModelUpdate movieModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _movieService.UpdateMovieAsync(id, movieModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/Movies/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Movie", Description = "Deletes an existing Movie by its ID.")]
        [SwaggerResponse(204, "Movie deleted successfully.")]
        [SwaggerResponse(404, "Movie not found.")]
        public async Task<ActionResult> DeleteMovie(Guid id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
