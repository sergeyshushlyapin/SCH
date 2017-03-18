using Microsoft.Extensions.DependencyInjection;
using Sitecore.Configuration;
using Sitecore.Hypermedia.Services;

namespace Sitecore.Hypermedia.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomServices(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IItemWorkflowService>(x =>
                new ItemWorkflowService(Factory.GetDatabase("master")));

            serviceCollection.AddTransient<IWorkflowService>(x =>
                new WorkflowService(Factory.GetDatabase("master")));

            return serviceCollection;
        }
    }
}
