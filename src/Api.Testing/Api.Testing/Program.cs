using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Api.Testing
{
    public class Program
    {
        private static bool IsDevelopment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            builder.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            IHost host = Host.CreateDefaultBuilder()
                        .ConfigureServices((context, services) =>
                        {
                            services.AddHttpClient();
                            services.AddTransient<MyApplication>();
                            services.AddScoped<IBusinessLayer, BusinessLayer>();
                            services.AddSingleton<IDataAccessLayer, DataAccessLayer>();
                        })
                        .UseSerilog()
                        .Build();

            string result = string.Empty;

            MyApplication app = ActivatorUtilities.CreateInstance<MyApplication>(host.Services);

            result = await app.Run();

            Console.WriteLine(result);
        }
    }

    /// <summary>
    /// DI IHTTPClientFactory
    /// </summary>
    public class MyApplication
    {
        private readonly ILogger _logger;

        private readonly IBusinessLayer _business;

        private IHttpClientFactory _httpFactory { get; set; }

        public MyApplication(ILogger<MyApplication> logger, IBusinessLayer business, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _business = business;
            _httpFactory = httpFactory;
        }

        public async Task<string> Run()
        {
            _logger.LogInformation("MyApplication {applicationEvent} at {dateTime}", "Started", DateTime.UtcNow);

            var request = new HttpRequestMessage(HttpMethod.Get, "https://TheCodeBuzz.com");

            var client = _httpFactory.CreateClient();

            var response = await client.SendAsync(request);

            _business.PerformBusiness();

            _logger.LogInformation("MyApplication {applicationEvent} at {dateTime}", "Ended", DateTime.UtcNow);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return $"StatusCode: {response.StatusCode}";
            }
        }
    }

    public class BusinessLayer : IBusinessLayer
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IDataAccessLayer _dataAccess;

        public BusinessLayer(ILogger<BusinessLayer> logger, IConfiguration config, IDataAccessLayer dataAccess)
        {
            _config = config;
            _logger = logger;
            _dataAccess = dataAccess;
        }

        public IDataAccessLayer DataAccess => _dataAccess;

        public void PerformBusiness()
        {
            _logger.LogInformation($"BusinessLayer Started at {DateTime.UtcNow}");

            // Reading connection string
            var usr = _config.GetValue<string>("MailSettings:Username");
            var pwd = _config.GetValue<string>("MailSettings:Password");

            _logger.LogInformation("Username: {usr}, Password: {pw}", usr, pwd);

            // Perform Business Logic here
            _dataAccess.Create();

            _logger.LogInformation($"BusinessLayer Ended at {DateTime.UtcNow}");
        }
    }

    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public DataAccessLayer(ILogger<BusinessLayer> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        public void Create()
        {
            _logger.LogInformation($"DataAccessLayer Started at {DateTime.UtcNow}");

            // Reading connection string
            var cs = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            _logger.LogInformation("Connection String: {cs}", cs);

            // Perform DataAccess Layer here
            Thread.Sleep(100);

            _logger.LogInformation($"DataAccessLayer Ended at {DateTime.UtcNow}");
        }
    }

    public interface IDataAccessLayer
    {
        void Create();
    }

    public interface IBusinessLayer
    {
        void PerformBusiness();
    }
}