using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EFCoreUtility
{
    public static class EfCoreServiceCollectionExtension
    {
        public static IServiceCollection AddGenericRepository<TDbContext>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TDbContext : DbContext
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IRepository),
                serviceProvider =>
                {
                    TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                    return new Repository<TDbContext>(dbContext);
                },
                lifetime));

            services.Add(new ServiceDescriptor(
               typeof(IRepository<TDbContext>),
               serviceProvider =>
               {
                   TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
                   return new Repository<TDbContext>(dbContext);
               },
               lifetime));

            return services;
        }
    }
}
