using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRestful.Core.Dtos;
using MovieRestful.Core.Services;

namespace MovieRestful.API.Controllers
{
    public class TrendingsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly ITrendingService _trendingService;

        public TrendingsController(IMapper mapper, ITrendingService trendingService)
        {
            _mapper = mapper;
            _trendingService = trendingService;
        }

        [HttpGet("list-most-viewed-movies")]
        public async Task<ActionResult> ListMostViewedMovies()
        {
            var movies = await _trendingService.ListMostViewedMovies();

            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("list-top-rated-movies")]
        public async Task<ActionResult> ListTopRatedMovies()
        {
            var movies = await _trendingService.ListTopRatedMovies();

            var movieDtos = _mapper.Map<List<MovieDto>>(movies);
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }


    }
}
