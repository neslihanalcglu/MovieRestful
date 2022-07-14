using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Service.Services
{
    public class TrendingService : Service<Movie>, ITrendingService
    {
        public TrendingService(IGenericRepository<Movie> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<List<Movie>> ListMostViewedMovies()
        {
            var movies = (await GetAllAsync()).ToList();
            IEnumerable<Movie> query = from word in movies
                                       orderby word.ViewedMovieCount descending
                                       select word;
            query = query.ToList().Distinct();
            var newMovies = new List<Movie>();

            for (var i = 0; i < 50; i++)
                newMovies.Add(query.ToList()[i]);
            return newMovies;
        }

        public async Task<List<Movie>> ListTopRatedMovies()
        {
            var movies = (await GetAllAsync()).ToList();
            IEnumerable<Movie> query = from word in movies
                                       orderby word.vote_count descending
                                       select word;
            query = query.ToList().Distinct();
            var newMovies = new List<Movie>();

            for (var i = 0; i < 50; i++)
                newMovies.Add(query.ToList()[i]);
            return newMovies;
        }
    }
}
