using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using Topshelf;

namespace servicewithtopself
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();

            HostFactory.Run(x =>
            {                
                x.Service<CsvExporterService>(s =>
                {
                    s.ConstructUsing(settings =>
                    {
                        var provider = services.BuildServiceProvider();
                        return provider.GetService<CsvExporterService>();
                    });
                    s.WhenStarted((tc, hostControl) =>
                    {
                        tc.Start(hostControl);
                        return true;
                    });
                    s.WhenStopped((tc, hostControl) =>
                    {
                        tc.Stop(hostControl);
                        return true;
                    });

                });
                x.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
                x.SetServiceName("EX_SqlCsvExporter");
                x.SetDescription("EX_SqlCsvExporter");
                x.SetDisplayName("EX_SqlCsvExporter");                
            }
                );
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();
            services.AddSingleton(config);

            // required to run the application
            services.AddSingleton<CsvExporterService, CsvExporterService>();
            services.AddSingleton<ICsvExporter, CsvExporter>();            

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            return builder;
        }
    }
}
