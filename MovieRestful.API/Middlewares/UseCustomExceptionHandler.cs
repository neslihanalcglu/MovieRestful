using Microsoft.AspNetCore.Diagnostics;
using MovieRestful.Core.Dtos;
using MovieRestful.Service.Exceptions;
using System.Text.Json;

namespace MovieRestful.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app) 
        {
            app.UseExceptionHandler(config => // bu modeli kullanarak kendi modelimizi yapıyoruz
            {

                config.Run(async context => // sonlandırıcı middleware 
                {                           // buraya geldikten sonra exception varsa daha ileri gitmeyecek
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>(); // hatayı alıyoruz

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400, // client hatası
                        NotFoundExcepiton => 404, // sayfa bulunamadı
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;


                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);

                    // kendi middleware'imizi yazdığımız için json akendimiz dönüştürmeliyiz
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });

        }
    }
}
