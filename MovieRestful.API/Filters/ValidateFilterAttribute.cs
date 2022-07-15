using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MovieRestful.Core.Dtos;

namespace MovieRestful.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context) //Method çalışıyorken müdahale ediyoruz
        {
            if (!context.ModelState.IsValid) // ModelState.IsValid üzerinden hata var mı yok mu kontrol edilebilir.
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors) // bütün hataları aldık (hata sınıfı)
                    .Select(x => x.ErrorMessage).ToList(); // sınıfın içindeki hata mesajları

                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(400, errors));
                // client hatası olduğu için 400
            }
        }
    }
}
