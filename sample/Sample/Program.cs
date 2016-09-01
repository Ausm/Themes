using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot hostingConfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("hosting.json", optional: true)
                    .AddCommandLine(args)
                    .Build();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(hostingConfig)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
