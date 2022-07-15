using FluentValidation;
using MovieRestful.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Service.Validations
{
    public class MovieDtoValidator:AbstractValidator<MovieDto>
    {
        public MovieDtoValidator()
        {
            RuleFor(x => x.original_title).NotNull().WithMessage("{PropertyName} is required"). 
                NotEmpty().WithMessage("{PropertyName} is required"); 

            RuleFor(x => x.title).NotNull().WithMessage("{PropertyName} is required"). // title alanı null olamaz
                NotEmpty().WithMessage("{PropertyName} is required"); //title boş bırakılamaz

            // Duruma ve ihtiyaca göre yeni validasyonlar FluentValidation dökümantasyonuna bakılarak eklenebilir.
        }
    }
}
