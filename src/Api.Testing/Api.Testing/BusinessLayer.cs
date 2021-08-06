using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Api.Testing
{
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
}