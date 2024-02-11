using RandomData.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using RandomData.Service.Services.Interface;
using RandomData.Service.Commands.Interfaces;
using RandomData.Service.Commands;
using RandomData.Service.Queries.Interfaces;
using RandomData.Service.Queries;
using System.Reflection;

namespace RandomData.Service
{
    public static class ServiceLayerDependenciesExtension
    {
        public static void AddServiceLayerDependencies(this IServiceCollection services)
        {


            services.AddQueries();
            services.AddCommands();
            services.AddScoped<IRandomDataService, RandomDataService>();
        }

        public static IServiceCollection AddQueries(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.Scan(s => s.FromAssemblies(assembly)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();

            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.Scan(s => s.FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
            return services;
        }
    }
}
