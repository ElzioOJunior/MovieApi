using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechChallenge.Application.Models.Movie
{
    public class MovieInterval
    {
        public string? Producer { get; set; }
        public int Interval { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }
    }

    public class PrizeIntervals
    {
        public List<MovieInterval> Min { get; set; }
        public List<MovieInterval> Max { get; set; }

        public PrizeIntervals()
        {
            Min = new List<MovieInterval>();
            Max = new List<MovieInterval>();
        }
    }
}
