using System.Net;
using CsvHelper.Configuration;
using TechChallenge.Domain.Entities;

public class MovieMap : ClassMap<Movie>
{
    public MovieMap()
    {
        Map(m => m.Year).Name("year");
        Map(m => m.Title).Name("title");
        Map(m => m.Studios).Name("studios");
        Map(m => m.Producers).Name("producers");
        Map(m => m.Winner).Name("winner");
    }
}
