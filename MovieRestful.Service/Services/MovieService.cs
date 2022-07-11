using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Service.Services
{
    public class MovieService : Service<Movie>, IMovieService
    {
        public MovieService(IGenericRepository<Movie> repository, DatabaseContext context, IUnitOfWork unitOfWork) : base(repository, context, unitOfWork)
        {
        }
    }
}
