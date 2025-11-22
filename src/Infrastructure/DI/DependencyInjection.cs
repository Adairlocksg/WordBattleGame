using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WordBattle.Domain.Entities.Categories;
using WordBattle.Infrastructure.Persistence;
using WordBattle.Infrastructure.Repositories;

namespace WordBattle.Infrastructure.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WordBattleDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            return services;
        }
    }
}
