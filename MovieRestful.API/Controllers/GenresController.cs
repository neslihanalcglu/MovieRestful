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
        private readonly IGenresService _genresService;

        public GenresController(IMapper mapper, IGenresService genresService)
        {
            _mapper = mapper;
            _genresService = genresService;
        }

        [HttpGet]
        public async Task<IActionResult> ListGenres()
        {
            var genres = await _genresService.ListGenres();
            return CreateActionResult(CustomResponseDto<List<string>>.Success(200, genres));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateGenre(long id, string genreId, string genreName)
        {
            var movie = await _genresService.UpdateGenre(id, genreId, genreName);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return CreateActionResult(CustomResponseDto<MovieDto>.Success(200, movieDto));
        }

    }
}
