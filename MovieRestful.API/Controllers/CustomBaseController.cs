using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRestful.Core.Dtos;

namespace MovieRestful.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        //endpointlerin geriye döneceği değerleri her birine tek tek oluşturmak yerine tek bir yerden kontrol ediliyor
        [NonAction]//endpont olmadığını belirtmek için, çünkü kendi içinde kullanıyoruz
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            //response ın StatusCodu'ü ne ise o dönecek
             
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };


        }
    }
}
