using DocumentFormat.OpenXml.Drawing.Diagrams;
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
        private readonly IMovieRepository _movieRepository;
        public MovieService(IGenericRepository<Movie> repository, DatabaseContext context, IUnitOfWork unitOfWork, IMovieRepository movieRepository) : base(repository, context, unitOfWork)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<Movie>> GetMovieListForGenreAsync(string input)
        {
            var movies = Where(x => x.genres.ToLower().Contains(input.ToLower())).ToList();
            return movies;
            //return await _movieRepository.GetMovieListForGenre(input);
        }

        public async Task<List<Movie>> GetMovieListForRate(int input)
        {
            var movies = Where(x => x.vote_count.Equals(input)).ToList();
            return movies;
        }

        public async Task<List<Movie>> GetMovieListForReleaseDate(string input)
        {
            var movies = Where(x => x.release_date.ToString().StartsWith(input)).ToList();
            return movies;
        }

        public async Task<List<Movie>> Search(string? title, int? rate, string? year)
        {
            var movies = new List<Movie>();

            if (title != null && rate != null && year != null)
            {
                movies = Where(x =>
                    (x.title.ToLower().Contains(title)) &&
                    (x.vote_count.Equals(rate)) &&
                    (x.release_date.ToString().StartsWith(year))
                ).ToList();
            }

            else if (title != null && rate != null)
            {
                movies = Where(x =>
                    (x.title.ToLower().Contains(title)) &&
                    (x.vote_count.Equals(rate))
                ).ToList();
            }

            else if (title != null && year != null)
            {
                movies = Where(x =>
                    (x.title.ToLower().Contains(title)) &&
                    (x.release_date.ToString().StartsWith(year))
                ).ToList();
            }

            else if (rate != null && year != null)
            {
                movies = Where(x =>
                    (x.vote_count.Equals(rate)) &&
                    (x.release_date.ToString().StartsWith(year))
                ).ToList();
            }

            else if (title != null)
            {
                movies = Where(x =>
                    x.title.ToLower().Contains(title)
                ).ToList();
            }

            else if (rate != null)
            {
                movies = Where(x =>
                    x.vote_count.Equals(rate)
                ).ToList();
            }

            else if (year != null)
            {
                movies = Where(x =>
                    x.release_date.ToString().StartsWith(year)
                ).ToList();
            }
            else
                movies = (await GetAllAsync()).ToList();
            return movies;
        }
    }
}
