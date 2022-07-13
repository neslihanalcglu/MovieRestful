using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRestful.Core.Dtos;
using MovieRestful.Core.Models;
using MovieRestful.Core.Services;

namespace MovieRestful.API.Controllers
{
    
    public class MoviesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;

        public MoviesController(IMapper mapper, IMovieService movieService)
        {
            _mapper = mapper;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult> GetListAsync(int maxResultCount)
        {
            var movies = await _movieService.GetAllResultAsync(maxResultCount);

            var movieDtos = _mapper.Map<List<MovieDto>>(movies.ToList());//Liste IEnumerable döndüğü için listeye çevrildi
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
            //return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMovieDetail(long id)
        {
            var movie = await _movieService.GetMovieAsync(id);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(CustomResponseDto<MovieDto>.Success(200, movieDto));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(MovieDto input)
        {
            var movie = await _movieService.AddAsync(_mapper.Map<Movie>(input));

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(CustomResponseDto<MovieDto>.Success(201, movieDto));// 201-Created
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(MovieDto input)
        {
            await _movieService.UpdateAsync(_mapper.Map<Movie>(input));
            return Ok(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            await _movieService.RemoveAsync(movie);

            return Ok(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("get-movie-list-for-genre")]
        public async Task<ActionResult> GetMovieListForGenre(string input, int maxResultCount)
        {
            var movies=await _movieService.GetMovieListForGenreAsync(input,maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("get-movie-list-for-rate")]
        public async Task<ActionResult> GetMovieListForRate(int input, int maxResultCount)
        {
            var movies = await _movieService.GetMovieListForRate(input,maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("get-movie-list-for-release-date")]
        public async Task<ActionResult> GetMovieListForReleaseDate(string input, int maxResultCount)
        {
            var movies = await _movieService.GetMovieListForReleaseDate(input, maxResultCount);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search(string? title, int? rate, string? year)
        {
            var movies = await _movieService.Search(title,rate,year);
            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }
    }
}
