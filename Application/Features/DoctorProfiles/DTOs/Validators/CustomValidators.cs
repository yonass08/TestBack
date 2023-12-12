using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Features.DoctorProfiles.DTOs.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, DateTime> YearMonthDate<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
        {
            return ruleBuilder.Must(dateTime => dateTime.ToString("yyyy-MM-dd") == dateTime.ToString("yyyy-MM-dd"))
                .WithMessage("{PropertyName} must be in the format 'YYYY-MM-DD'");
        }

         public static bool IsValidFileExtension(IFormFile doctorPhoto)
    {
        if (doctorPhoto == null)
        {
            return true; // Skip validation if file is not provided
        }

        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        var extension = Path.GetExtension(doctorPhoto.FileName);
        return validExtensions.Contains(extension.ToLower());
    }
    }


}