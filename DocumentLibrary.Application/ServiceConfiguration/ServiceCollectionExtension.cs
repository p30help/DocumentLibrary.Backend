using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DocumentsLibrary.Application.ServiceConfiguration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {            
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
    }
}
