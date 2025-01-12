using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Application.Models
{
    public class MovieModelBase
    {

        [Required(ErrorMessage = "Year is required")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 150 characters")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "Studios is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Studios must be between 3 and 150 characters")]
        public required string Studios { get; set; }

        [Required(ErrorMessage = "Producers is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Producers must be between 3 and 150 characters")]
        public required string Producers { get; set; }

        [Required(ErrorMessage = "Winner is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Winner must be between 3 and 150 characters")]
        public required string Winner { get; set; }
    }
}
