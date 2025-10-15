using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Infrastructure.Persistance.Context;
using EksiSozluk.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Infrastructure.Persistance.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EksiSozlukContext>(conf =>
            {
                var connStr = configuration["EksiSozlukDbConnectionString"].ToString();
                conf.UseSqlServer(connStr);
            });


            // bu kısmı sonradan ekledik seedData olusturup db'ye eklemek için.
            //var seedData = new SeedData();
            //seedData.SeedAsync(configuration).GetAwaiter().GetResult();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IEntryCommentRepository, EntryCommentRepository>();
            services.AddScoped<IEmailConfirmationRepository, EmailConfirmationRepository>();

            return services;

        }
    }
}
