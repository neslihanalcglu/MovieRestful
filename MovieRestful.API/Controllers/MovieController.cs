using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRestful.Core.Dtos;
using MovieRestful.Core.Models;
using MovieRestful.Core.Services;

namespace MovieRestful.API.Controllers
{
    public class MovieController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IService<Movie> _service;

        public MovieController(IMapper mapper, IService<Movie> service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetListAsync()
        {
            var movies = await _service.GetAllAsync();

            var movieDtos = _mapper.Map<List<MovieDto>>(movies.ToList());//Liste IEnumerable döndüğü için listeye çevrildi
            return Ok(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
            //return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(long id)
        {
            var movie = await _service.GetByIdAsync(id);

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(CustomResponseDto<MovieDto>.Success(200, movieDto));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(MovieDto input)
        {
            var movie = await _service.AddAsync(_mapper.Map<Movie>(input));

            var movieDto = _mapper.Map<MovieDto>(movie);
            return Ok(CustomResponseDto<MovieDto>.Success(201, movieDto));// 201-Created
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync(MovieDto input)
        {
            await _service.UpdateAsync(_mapper.Map<Movie>(input));
            return Ok(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var movie = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(movie);

            return Ok(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
