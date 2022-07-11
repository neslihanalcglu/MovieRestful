using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Core.Models
{
    public class Trending
    {
        public long id { get; set; }
        public int ViewedMovieCount { get; set; }
        public Movie Movie { get; set; }

        public long MovieId { get; set; }
    }
}
