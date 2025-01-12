using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Application.Models.Movie
{
    public class MovieModel : MovieModelBase
    {
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }
      
    }
}
