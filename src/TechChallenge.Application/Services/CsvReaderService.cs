using CsvHelper;
using CsvHelper.Configuration;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Services
{
    public class CsvReaderService : ICsvReaderService
    {
        private readonly IMovieRepository _movieRepository;

        public CsvReaderService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task ImportMoviesFromCsvAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Arquivo CSV não encontrado.", filePath);
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", 
            }))
            {
                csv.Context.RegisterClassMap<MovieMap>();

                var records = csv.GetRecords<Movie>().Select(record =>
                                                 {
                                                     record.Id = Guid.NewGuid();
                                                     return record;
                                                 })
                                                 .ToList();

                await _movieRepository.AddRangeAsync(records);
            }
        }
    }
}
