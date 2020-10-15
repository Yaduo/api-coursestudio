using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using NLog.Web;

namespace CourseStudioManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
		           .ConfigureServices(services => services.AddAutofac())
		           .ConfigureAppConfiguration((hostingContext, config) =>
                   {
                       var env = hostingContext.HostingEnvironment;
                       config.AddJsonFile("AppConfig/appsettings.json", optional: true, reloadOnChange: true)
			                 .AddJsonFile($"AppConfig/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                   })
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(LogLevel.Trace);
                   })
                   .UseNLog()
                   .UseSetting("detailedErrors", "true")
		           .UseStartup<Startup>()
		           .CaptureStartupErrors(true)
		           .Build();
    }
}
