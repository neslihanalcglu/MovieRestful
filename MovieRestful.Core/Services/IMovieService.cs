using MovieRestful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Core.Services
{
    public interface IMovieService:IService<Movie>
    {
        Task<Movie> GetMovieAsync(long id);
        Task<List<Movie>> GetMovieListForGenreAsync(string input);
        Task<List<Movie>> GetMovieListForRate(int input);
        Task<List<Movie>> GetMovieListForReleaseDate(string input);
        Task<List<Movie>> Search(string? title, int? rate, string? year);
        Task<List<string>> ListGenres();
    }

    
}
