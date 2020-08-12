using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Compact;

namespace school_personnel_management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = BuildWebHost(args);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var post = Convert.ToInt32(configuration["AppSettings:LogstashPort"]);
            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.WriteTo.Udp(configuration["Logging:LogStash:LogstashAddress"], Convert.ToInt32(configuration["Logging:LogStash:LogstashPort"]), new CompactJsonFormatter())
                .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                .CreateLogger();


            webHost.Run();
        }


        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((config) => { config.ClearProviders(); })
                .UseSerilog()
                .UseUrls("http://0.0.0.0:5000")
                .Build();
        }
    }
}
