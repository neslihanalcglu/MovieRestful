using MovieRestful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Core.Services
{
    public interface IGenresService : IService<Movie>
    {
        Task<List<string>> ListGenres();
        Task<Movie> UpdateGenre(long id, string genreId, string genreName);
    }
}
