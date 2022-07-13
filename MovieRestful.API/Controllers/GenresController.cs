using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRestful.Core.Dtos;
using MovieRestful.Core.Services;

namespace MovieRestful.API.Controllers
{
    public class GenresController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;

        public GenresController(IMapper mapper, IMovieService movieService)
        {
            _mapper = mapper;
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult> ListGenres()
        {
            var genres = await _movieService.ListGenres();
            return Ok(CustomResponseDto<List<string>>.Success(200, genres));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateGenre(long id, string genreId, string genreName)
        {
            var movie = await _movieService.UpdateGenre(id, genreId, genreName);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(CustomResponseDto<MovieDto>.Success(200, movieDto));
        }

    }
}
