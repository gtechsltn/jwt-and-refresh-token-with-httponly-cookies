using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Api.Testing
{
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
}