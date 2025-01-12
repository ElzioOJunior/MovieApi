using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge.Application.Interfaces
{
    public interface ICsvReaderService
    {
        Task ImportMoviesFromCsvAsync(string filePath);
    }

}
