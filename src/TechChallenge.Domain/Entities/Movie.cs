namespace TechChallenge.Domain.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public required string Year { get; set; }
        public required string Title { get; set; }
        public required string Studios { get; set; }
        public required string Producers { get; set; }
        public required string Winner { get; set; }
    }
}
