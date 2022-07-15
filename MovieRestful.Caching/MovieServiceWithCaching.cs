using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Service.Exceptions;
using MovieRestful.Service.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Caching
{
    // Proxy Design Pattern
    public class MovieServiceWithCaching : IMovieService
    {
        private const string CacheMovieKey = "moviesCache"; // key
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRedisHelper _redisHelper;

        public MovieServiceWithCaching(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, IMovieRepository movieRepository, IRedisHelper redisHelper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _movieRepository = movieRepository;
            _redisHelper = redisHelper;

            if (_memoryCache.TryGetValue(CacheMovieKey, out _))
            {
                //_redisHelper.SetKeyAsync(CacheMovieKey, _movieRepository.GetAll().ToList());
                _memoryCache.Set(CacheMovieKey, _movieRepository.GetAll().ToList());
            }
        }

        public async Task<Movie> AddAsync(Movie entity)
        {
            await _movieRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllMoviesAsync();
            return entity;
        }

        public async Task<IEnumerable<Movie>> AddRangeAsync(IEnumerable<Movie> entities)
        {
            await _movieRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllMoviesAsync();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Movie, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> CreateMovieAsync(Movie input)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Movie>>(CacheMovieKey));
        }

        public Task<List<Movie>> GetAllMoviesUsingRedisCache()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAllResultAsync(int maxResultCount = 0)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetByIdAsync(long id)
        {
            var movie = _memoryCache.Get<List<Movie>>(CacheMovieKey).FirstOrDefault(x => x.id == id);

            if (movie == null)
            {
                throw new NotFoundExcepiton($"{typeof(Movie).Name}({id}) not found");
            }
            return Task.FromResult(movie);
        }

        public Task<Movie> GetMovieAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetMovieListForGenreAsync(string input, int maxResultCount)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetMovieListForRate(int input, int maxResultCount)
        {
            throw new NotImplementedException();
        }

        public Task<List<Movie>> GetMovieListForReleaseDate(string input, int maxResultCount)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Movie entity)
        {
            _movieRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllMoviesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<Movie> entities)
        {
            _movieRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllMoviesAsync();
        }

        public Task<List<Movie>> Search(string? title, int? rate, string? year)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Movie entity)
        {
            _movieRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllMoviesAsync();
        }

        public IQueryable<Movie> Where(Expression<Func<Movie, bool>> expression)
        {
            return _memoryCache.Get<List<Movie>>(CacheMovieKey).Where(expression.Compile()).AsQueryable();
        }


        public async Task CacheAllMoviesAsync() // tüm datayı çekip cacheliyoruz
        {
            _memoryCache.Set(CacheMovieKey, _movieRepository.GetAll().ToList());

        }
    }
}
