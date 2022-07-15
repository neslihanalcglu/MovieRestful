using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using MovieRestful.Core.Dtos;
using MovieRestful.Core.Models;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Service.Redis;
using Newtonsoft.Json;
using System.Text;

namespace MovieRestful.API.Controllers
{
    
    public class MoviesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;
        private readonly IRedisHelper _redisHelper;

        public MoviesController(IMapper mapper, IMovieService movieService, IRedisHelper redisHelper)
        {
            _mapper = mapper;
            _movieService = movieService;
            _redisHelper = redisHelper;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync(int maxResultCount)
        {
            var movies = await _movieService.GetAllResultAsync(maxResultCount);

            var movieDtos = _mapper.Map<List<MovieDto>>(movies.ToList());//Liste IEnumerable döndüğü için listeye çevrildi
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("redis")]
        public async Task<IActionResult> GetAllMoviesUsingRedisCache()
        {
            var movies = await _movieService.GetAllMoviesUsingRedisCache();

            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieDetail(long id)
        {
            var movie = await _movieService.GetMovieAsync(id);
            var movieId = await _redisHelper.GetKeyAsync("movieId");
            var movieDto = _mapper.Map<MovieDto>(movie);
            return CreateActionResult(CustomResponseDto<MovieDto>.Success(200, movieDto));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(MovieDto input)
        {
            var movie = await _movieService.CreateMovieAsync(_mapper.Map<Movie>(input));
            await _redisHelper.SetKeyAsync("movieId", movie.id.ToString());
            var movieDto = _mapper.Map<MovieDto>(movie);
            return CreateActionResult(CustomResponseDto<MovieDto>.Success(201, movieDto)); // 201-Created
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(MovieDto input)
        {
            await _movieService.UpdateAsync(_mapper.Map<Movie>(input));
            return CreateActionResult(CustomResponseDto<MovieDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            await _movieService.RemoveAsync(movie);
            var movieId = await _redisHelper.GetKeyAsync("movieId");
            return CreateActionResult(CustomResponseDto<MovieDto>.Success(204));
        }

        [HttpGet("get-movie-list-for-genre")]
        public async Task<IActionResult> GetMovieListForGenre(string input, int maxResultCount)
        {
            var movies=await _movieService.GetMovieListForGenreAsync(input,maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("get-movie-list-for-rate")]
        public async Task<IActionResult> GetMovieListForRate(int input, int maxResultCount)
        {
            var movies = await _movieService.GetMovieListForRate(input,maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("get-movie-list-for-release-date")]
        public async Task<IActionResult> GetMovieListForReleaseDate(string input, int maxResultCount)
        {
            var movies = await _movieService.GetMovieListForReleaseDate(input, maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? title, int? rate, string? year)
        {
            var movies = await _movieService.Search(title,rate,year);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }
        

    }
}
