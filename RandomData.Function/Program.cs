using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomData.Service;
using System;
using System.IO;

namespace RandomData.Function
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureAppConfiguration((hostContext, configBuilder) =>
            {
                configBuilder 
                .SetBasePath(Environment.CurrentDirectory)
                .AddEnvironmentVariables()
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .Build();
                   
            })
            .ConfigureServices(services =>
            {
                services.AddServiceLayerDependencies();
                services.AddHttpClient("RandomDataApi", httpClient =>
                {
                    httpClient.BaseAddress = new Uri("https://api.publicapis.org/");
                });
            })
            .Build();

            host.Run();
        }
    }
}

