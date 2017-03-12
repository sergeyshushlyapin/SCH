using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Hypermedia.Extensions;
using Sitecore.Services.Infrastructure.Sitecore.DependencyInjection;

namespace Sitecore.Hypermedia.DependencyInjection
{
    /// <summary>
    /// Configures List Manager WebAPI controllers and all their dependencies.
    /// </summary>
    internal class CustomServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            var assemblies = new[]
            {
                GetType().Assembly
            };

            serviceCollection
                .AddWebApiControllers(assemblies);

            serviceCollection
                .AddCustomServices();
        }
    }
}
