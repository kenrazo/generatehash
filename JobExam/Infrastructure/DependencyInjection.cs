using Infrastructure.RabbitMq;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services)
        {
            return services.AddDbContext<HashDbContext>(options => options
                .UseSqlServer("Server=localhost, 1433;Initial Catalog=hash;Persist Security Info=False;User ID=sa;Password=8Waystop;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;", b=> b.MigrationsAssembly("JobExam")),
                ServiceLifetime.Transient);
        }

        public static IServiceCollection InjectRepository(this IServiceCollection services)
        {
            services.AddScoped<IHashRepository, HashRepository>();

            return services;
        }

        public static IServiceCollection InjectRmq(this IServiceCollection services)
        {
            services.AddScoped<IRabbitmqService, RabbitMqService>();
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();

            return services;
        }
    }
}
