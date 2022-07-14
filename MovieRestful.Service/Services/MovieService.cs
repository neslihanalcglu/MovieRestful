using DocumentFormat.OpenXml.Drawing.Diagrams;
using IEnumerable.ForEach;
using Microsoft.AspNetCore.Authorization;
using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Repository;
using MovieRestful.Service.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Service.Services
{
    [Authorize]
    public class MovieService : Service<Movie>, IMovieService
    {
        public MovieService(IGenericRepository<Movie> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var movie = await GetByIdAsync(id);
            movie.ViewedMovieCount++;
            await UpdateAsync(movie);
            return movie;
        }

        public async Task<List<Movie>> GetMovieListForGenreAsync(string input,int maxResultCount)
        {
            var movies = Where(x => x.genres.ToLower().Contains(input.ToLower())).ToList();
            var newMovies=new List<Movie>();
            if (maxResultCount != 0)
            {
                for (var i = 0; i < maxResultCount; i++)
                    newMovies.Add(movies[i]);
                return newMovies;
            }
            return movies;
            //return await _movieRepository.GetMovieListForGenre(input);
        }

        public async Task<List<Movie>> GetMovieListForRate(int input, int maxResultCount)
        {
            var movies = Where(x => x.vote_count.Equals(input)).ToList();
            var newMovies = new List<Movie>();
            if (maxResultCount != 0)
            {
                for (var i = 0; i < maxResultCount; i++)
                    newMovies.Add(movies[i]);
                return newMovies;
            }
            return movies;
        }

        public async Task<List<Movie>> GetMovieListForReleaseDate(string input, int maxResultCount)
        {
            var movies = Where(x => x.release_date.ToString().StartsWith(input)).ToList();
            var newMovies = new List<Movie>();
            if (maxResultCount != 0)
            {
                for (var i = 0; i < maxResultCount; i++)
                    newMovies.Add(movies[i]);
                return newMovies;
            }
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

        public async Task<List<string>> ListGenres()
        {
            var movies =await GetAllAsync();
            var genres = new List<string>();
            
            movies.ForEach(x => { genres.Add(x.genres); });
            return genres;
        }

        public async Task<Movie> UpdateGenre(long id, string genreId,string genreName)
        {
            var movies = await GetByIdAsync(id);
            var newGenres="[{'id':"+ genreId + ",'name':"+ genreName +"}]";
            movies.genres = newGenres;
            await UpdateAsync(movies);
            return movies;
        }

    }
}
