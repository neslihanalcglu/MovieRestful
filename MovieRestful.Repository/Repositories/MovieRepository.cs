using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Repository.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(DatabaseContext context) : base(context)
        {
        }
        //public async Task<List<Movie>> GetMovieListForGenre(string input)
        //{
        //    var movies = Where(x => x.genres.ToLower().Contains(input.ToLower())).ToList();
        //    return movies;
        //}

    }
}
