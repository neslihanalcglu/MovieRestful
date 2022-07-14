using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieRestful.API.BackgroundServices;
using MovieRestful.Core.Repositories;
using MovieRestful.Core.Services;
using MovieRestful.Core.UnitOfWorks;
using MovieRestful.Repository;
using MovieRestful.Repository.Repositories;
using MovieRestful.Repository.UnitOfWorks;
using MovieRestful.Service.Mapping;
using MovieRestful.Service.Redis;
using MovieRestful.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// UnitOfWork Implement
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Repositories Implement
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IMovieRepository), typeof(MovieRepository));

// Services Implement
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped(typeof(IMovieService), typeof(MovieService));
builder.Services.AddScoped(typeof(ITrendingService), typeof(TrendingService));

// AutoMapper Implement
builder.Services.AddAutoMapper(typeof(MapProfile));

// Database Implement
builder.Services.AddDbContext<DatabaseContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("Connection"), option =>
    {
        //dinamik olmasý için DatabaseContext in Assembly sini alacak. MovieRestful.Repository ismini getirdi.
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(DatabaseContext)).GetName().Name);
    });

});


// Redis Implement
builder.Services.AddSingleton<IRedisHelper, RedisHelper>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHangfire(config =>
                config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));

//Hangfire.RecurringJob.AddOrUpdate(() => Console.WriteLine("Recurring jobs tetiklendi!"), Hangfire.Cron.MinuteInterval(1));


// Hosted Service implement
builder.Services.AddHostedService<MovieBGService>();

//swagger yapýlandýrmasý
var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};

var contact = new OpenApiContact()
{
    Name = "Neslihan Alýcýoðlu",
    Email = "neslihanalcglu@gmail.com",
    Url = new Uri("https://github.com/neslihanalcglu")
};

var license = new OpenApiLicense()
{
    Name = "Free License",
    Url = new Uri("https://github.com/neslihanalcglu")
};

var info = new OpenApiInfo()
{
    Version = "v1",
    Title = "Movie Restful API",
    Description = "Arvato .Net Bootcamp Graduation Project",
    TermsOfService = new Uri("http://www.example.com"),
    Contact = contact,
    License = license
};

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("Bearer", securityScheme);
    o.AddSecurityRequirement(securityReq);
});

// JWt yapýlandýrmasý 
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

//Kimlik doðrulama ve yetkilendirme aktifleþtirme
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();

//JWT'nin kullanýlabilmesi için gerekli olan endpoint ekleniyor.
app.MapPost("/security/getToken", [AllowAnonymous] (UserDto user) =>
{

    if (user.UserName == "admin@mail.com" && user.Password == "Password123")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // JWT tokenlarýný oluþturmaktan sorumlu JWT token tanýmlandý
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        // Key appsettings.json' dan okunuyor.
        var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", "1"),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            
            //Yenilenme süresi (Refresh token)
            Expires = DateTime.UtcNow.AddMinutes(10),
            Audience = audience,
            Issuer = issuer,
            // Token ý çözebilmek için kullanýlan þifreleme algoritma bilgileri
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        var jwtToken = jwtTokenHandler.WriteToken(token);

        return Results.Ok(jwtToken);
    }
    else
    {
        return Results.Unauthorized();
    }
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//Kimlik doðrulama için bir dto bekliyor bu sebeple burada bir dto oluþturuldu. 
record UserDto(string UserName, string Password);

