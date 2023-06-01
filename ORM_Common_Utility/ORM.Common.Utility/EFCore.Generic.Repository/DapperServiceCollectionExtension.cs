using EFCoreUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EFCore.Generic.Repository
{
    public static class DapperServiceCollectionExtension
    {
        public static IServiceCollection AddDapperGenericRepository<T>(
           this IServiceCollection services,
           ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : IDbConnection
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Add(new ServiceDescriptor(
                typeof(IDapperRepository<T>),
                serviceProvider =>
                {
                    T dbContext = ActivatorUtilities.CreateInstance<T>(serviceProvider);
                    return new DapperRepository<T>(dbContext);
                },
                lifetime));

            return services;
        }
    }
}
