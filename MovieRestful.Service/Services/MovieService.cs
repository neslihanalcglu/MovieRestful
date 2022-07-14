using DocumentFormat.OpenXml.Drawing.Diagrams;
using IEnumerable.ForEach;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Repository;
using MovieRestful.Service.Redis;
using Newtonsoft.Json;
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
        private readonly IDistributedCache _distributedCache;
        public MovieService(IGenericRepository<Movie> repository, IUnitOfWork unitOfWork, IDistributedCache distributedCache) : base(repository, unitOfWork)
        {
            _distributedCache = distributedCache;
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var movie = await GetByIdAsync(id);
            movie.ViewedMovieCount++;
            await UpdateAsync(movie);
            return movie;
        }

        public async Task<List<Movie>> GetMovieListForGenreAsync(string input, int maxResultCount)
        {
            var movies = Where(x => x.genres.ToLower().Contains(input.ToLower())).ToList();
            var newMovies = new List<Movie>();
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

        
        public async Task<List<Movie>> GetAllMoviesUsingRedisCache()
        {
            var cacheKey = "movieList"; // Anahtarı kodun içinde dahili olarak ayarlıyoruz
            string serializedMovieList;
            List<Movie> movieList; // Boş bir Film Listesi tanımlıyoruz.
            var redisMovieList = await _distributedCache.GetAsync(cacheKey); //“movieList” anahtarını kullanarak Redis’ten veri almak için distributed önbellek nesnesine erişin.
            if (redisMovieList != null) //Anahtarın Redis’te bir değeri varsa, bunu bir Filmler listesine dönüştürün ve verileri geri gönderin.
            {
                serializedMovieList = Encoding.UTF8.GetString(redisMovieList); // Veriler Redis’te bir bayt dizisi olarak depolanacaktır. Bu stringi bir array’e dönüştüreceğiz.
                movieList = JsonConvert.DeserializeObject<List<Movie>>(serializedMovieList); // Dizeyi List türünde bir nesneye dönüştürür
            }
            else // Değer Redis’te yoksa, veritabanına efcore üzerinden erişin, verileri alın ve redis olarak ayarlayın.
            {
                movieList = (await GetAllAsync()).ToList();
                serializedMovieList = JsonConvert.SerializeObject(movieList); // NewtonSoft kullanarak bir film listesini bir dizeye dönüştürür.
                redisMovieList = Encoding.UTF8.GetBytes(serializedMovieList); //  string’i bir Bayt array’e dönüştürür. Bu array Redis’te depolanacak.
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)) // Burada önbelleğe alınmış nesnenin sona erme süresini ayarlanır.
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // Belirli bir süre boyunca istenmezse, önbelleğe alınmış bir nesne olarak sona erer.
                await _distributedCache.SetAsync(cacheKey, redisMovieList, options); // Son olarak önbelleği ayarlayın.
            }
            return movieList;
        }
    }
}
