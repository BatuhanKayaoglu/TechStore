using EksiSozluk.Api.Application.Cache;
using EksiSozluk.Api.WebApi.Middlewares.Filter.GlobalExceptionHandler;
using EksiSozluk.Api.WebApi.Middlewares.Filter.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;

namespace EksiSozluk.Api.WebApi.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddWebApiRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ExceptionMiddleware>();

            // Start FluentValidation CONFIGURATION 
            services.AddControllers().AddFluentValidation();
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            // End FluentValidation CONFIGURATION   


            // Redis Cache CONFIGURATION    
            string? redisConfiguration = configuration.GetSection("Redis").Value;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
            });

            return services;
        }
    }
}
