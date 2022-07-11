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
        Task<List<Movie>> GetMovieListForGenre(string input);
    }

    
}
