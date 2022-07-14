using IEnumerable.ForEach;
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
    public class GenresService : Service<Movie>, IGenresService
    {
        public GenresService(IGenericRepository<Movie> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<List<string>> ListGenres()
        {
            var movies = await GetAllAsync();
            var genres = new List<string>();

            movies.ForEach(x => { genres.Add(x.genres); });
            return genres;
        }

        public async Task<Movie> UpdateGenre(long id, string genreId, string genreName)
        {
            var movies = await GetByIdAsync(id);
            var newGenres = "[{'id':" + genreId + ",'name':" + genreName + "}]";
            movies.genres = newGenres;
            await UpdateAsync(movies);
            return movies;
        }
    }
}
