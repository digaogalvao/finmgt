using FinancialManagement.Application.Interfaces;
using FinancialManagement.Application.Services;
using FinancialManagement.Domain.Interfaces;
using FinancialManagement.Infrastructure.Context;
using FinancialManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext)
                            .Assembly.FullName)));

            services.AddScoped<ITransacaoService, TransacaoService>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
