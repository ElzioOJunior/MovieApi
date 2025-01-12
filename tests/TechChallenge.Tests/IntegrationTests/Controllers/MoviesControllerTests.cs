using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.API.Controllers;
using TechChallenge.Application.Interfaces;
using TechChallenge.Application.Models.Movie;
using Xunit;

public class MoviesControllerTests
{
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly MoviesController _controller;

    public MoviesControllerTests()
    {
        _mockMovieService = new Mock<IMovieService>();
        _controller = new MoviesController(_mockMovieService.Object);
    }

    [Fact]
    public async Task GetAllMovies_ReturnsOkResult_WithListOfMovies()
    {
        var movies = new List<MovieModel>();

        movies.Add(new MovieModel
        {
            Id = Guid.NewGuid(),
            Title = "New Movie",
            Producers = "New Producer",
            Studios = "New Studio",
            Winner = "New winner",
            Year = "1990"
        });

        _mockMovieService.Setup(service => service.GetAllMoviesAsync()).ReturnsAsync(movies);

        var result = await _controller.GetAllMovies();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMovies = Assert.IsType<List<MovieModel>>(okResult.Value);
        Assert.Single(returnMovies);
    }

    [Fact]
    public async Task GetMovieById_ReturnsOkResult_WithMovie()
    {
        var movieId = Guid.NewGuid();
        var movie = new MovieModel
        {
            Id = movieId,
            Title = "New Movie",
            Producers = "New Producer",
            Studios = "New Studio",
            Winner = "New winner",
            Year = "1990"
        };
        _mockMovieService.Setup(service => service.GetMovieByIdAsync(movieId)).ReturnsAsync(movie);

        var result = await _controller.GetMovieById(movieId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMovie = Assert.IsType<MovieModel>(okResult.Value);
        Assert.Equal(movieId, returnMovie.Id);
    }

    [Fact]
    public async Task GetMovieById_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        var movieId = Guid.NewGuid();
        _mockMovieService.Setup(service => service.GetMovieByIdAsync(movieId)).ThrowsAsync(new Exception("Movie not found"));

        var result = await _controller.GetMovieById(movieId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);

        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetProducersWithPrizeIntervals_ReturnsOkResult_WithPrizeIntervals()
    {

        var prizeIntervals = new PrizeIntervals
        {
            Min = new List<MovieInterval>
        {
            new MovieInterval
            {
                Producer = "Allan Carr",
                Interval = 3,
                PreviousWin = 1980,
                FollowingWin = 1983
            },
            new MovieInterval
            {
                Producer = "Frank Yablans",
                Interval = 5,
                PreviousWin = 1981,
                FollowingWin = 1986
            }
        },
            Max = new List<MovieInterval>
        {
            new MovieInterval
            {
                Producer = "Mitsuharu Ishii",
                Interval = 10,
                PreviousWin = 1982,
                FollowingWin = 1992
            }
        }
        };

        _mockMovieService.Setup(service => service.GetProducersWithPrizeIntervals()).ReturnsAsync(prizeIntervals);

        var result = await _controller.GetProducersWithPrizeIntervals();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnPrizeIntervals = Assert.IsType<PrizeIntervals>(okResult.Value);

        Assert.NotEmpty(returnPrizeIntervals.Min);
        Assert.NotEmpty(returnPrizeIntervals.Max);

        var minInterval = returnPrizeIntervals.Min.First();
        Assert.Equal("Allan Carr", minInterval.Producer);
        Assert.Equal(3, minInterval.Interval);
        Assert.Equal(1980, minInterval.PreviousWin);
        Assert.Equal(1983, minInterval.FollowingWin);
    }


    [Fact]
    public async Task CreateMovie_ReturnsCreatedResult_WithMovie()
    {
        var movieDto = new MovieModelInsert
        {
            Title = "New Movie",
            Producers = "New Producer",
            Studios = "New Studio",
            Winner = "New winner",
            Year = "1990"
        };

        var createdMovie = new MovieModel
        {
            Id = Guid.NewGuid(),
            Title = "Created Movie",
            Producers = "Created Producer",
            Studios = "Created Studio",
            Winner = "Created winner",
            Year = "1990"
        };

        _mockMovieService.Setup(service => service.CreateMovieAsync(movieDto)).ReturnsAsync(createdMovie);

        var result = await _controller.CreateMovie(movieDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnMovie = Assert.IsType<MovieModel>(createdResult.Value);
        Assert.Equal("Created Movie", returnMovie.Title);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsOkResult_WithUpdatedMovie()
    {
        var movieId = Guid.NewGuid();
        var movieDto = new MovieModelUpdate
        {
            Title = "New Movie",
            Producers = "New Producer",
            Studios = "New Studio",
            Winner = "New winner",
            Year = "1990"
        };

        var updatedMovie = new MovieModel
        {
            Id = movieId,
            Title = "Updated Movie",
            Producers = "Updated Producer",
            Studios = "Updated Studio",
            Winner = "Updated winner",
            Year = "1990"
        };
        _mockMovieService.Setup(service => service.UpdateMovieAsync(movieId, movieDto)).ReturnsAsync(updatedMovie);

        var result = await _controller.UpdateMovie(movieId, movieDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnMovie = Assert.IsType<MovieModel>(okResult.Value);
        Assert.Equal("Updated Movie", returnMovie.Title);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        var movieId = Guid.NewGuid();
        var movieDto = new MovieModelUpdate
        {
            Title = "Updated Movie",
            Producers = "Updated Producer",
            Studios = "Updated Studio",
            Winner = "Updated winner",
            Year = "1991"
        };

        _mockMovieService.Setup(service => service.UpdateMovieAsync(movieId, movieDto))
                         .ThrowsAsync(new Exception("Movie not found"));

        var result = await _controller.UpdateMovie(movieId, movieDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result); 

        Assert.Equal(404, notFoundResult.StatusCode); 

    }



    [Fact]
    public async Task DeleteMovie_ReturnsNoContent()
    {
        var movieId = Guid.NewGuid();

        _mockMovieService.Setup(service => service.DeleteMovieAsync(movieId))
                         .ReturnsAsync(true);  

        var result = await _controller.DeleteMovie(movieId);

        Assert.IsType<NoContentResult>(result);
    }


    [Fact]
    public async Task DeleteMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        var movieId = Guid.NewGuid();
        _mockMovieService.Setup(service => service.DeleteMovieAsync(movieId)).ThrowsAsync(new Exception("Movie not found"));

        var result = await _controller.DeleteMovie(movieId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal(404, notFoundResult.StatusCode);
    }

}
