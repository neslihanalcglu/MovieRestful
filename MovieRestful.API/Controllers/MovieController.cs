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

        public async Task<ActionResult> GetList()
        {
            var movies = await _service.GetAllAsync();
            
            var movieDtos= _mapper.Map<List<MovieDto>>(movies.ToList());//Liste IEnumerable döndüğü için listeye çevrildi
            return Ok( CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
            //return CreateActionResult(CustomResponseDto<List<MovieDto>>.Success(200, movieDtos));
        }
    }
}
