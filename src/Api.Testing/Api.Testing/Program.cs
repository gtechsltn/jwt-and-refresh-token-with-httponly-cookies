using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Api.Testing
{
    /// <summary>
    /// Tools->Options->Debugging->Automatically close the console when debugging stops.
    /// </summary>
    public class Program
    {
        private static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env}.json", true)
            .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                //.WriteTo.File("log.txt")
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("ASPNETCORE_ENVIRONMENT: {env}", env);

            IHost host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        {
                            services.AddHttpClient();
                            services.AddTransient<MyConsoleApplication>();
                            services.AddScoped<IBusinessLayer, BusinessLayer>();
                            services.AddSingleton<IDataAccessLayer, DataAccessLayer>();
                        })
                        .UseSerilog()
                        .Build();

            string result = string.Empty;

            MyConsoleApplication app = ActivatorUtilities.CreateInstance<MyConsoleApplication>(host.Services);

            //await app.ManageApi_AdminUser_StoreManagement();
            await app.ShopApi_StoreUser_Management();
        }
    }
}