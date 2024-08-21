using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Application.Services;
using LibraryManagementAssignment.Domain.Interfaces;
using LibraryManagementAssignment.Infrastructure.Data;
using LibraryManagementAssignment.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementAssignment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LibraryContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookServices, BookServices>();
            return services;
        }
    }
}
