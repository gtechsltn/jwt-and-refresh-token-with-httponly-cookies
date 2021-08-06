using Api.Testing.Dtos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Api.Testing
{
    /// <summary>
    /// DI IHTTPClientFactory
    /// </summary>
    public class MyConsoleApplication
    {
        private readonly ILogger _logger;

        private readonly IBusinessLayer _business;

        private IHttpClientFactory _httpFactory { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public MyConsoleApplication(ILogger<MyConsoleApplication> logger, IBusinessLayer business, IHttpClientFactory httpFactory)
        {
            _logger = logger;
            _business = business;
            _httpFactory = httpFactory;
        }

        public async Task ManageApi_AdminUser_StoreManagement()
        {
            HttpClient client;
            MediaTypeWithQualityHeaderValue contentType;
            HttpResponseMessage response;
            string baseUrl = "https://localhost:44321";
            string apiResult = "";

            _logger.LogInformation("{applicationName} {applicationEvent} at {dateTime}", nameof(MyConsoleApplication), "Started", DateTime.UtcNow);

            //Authentication: Get JWT
            client = _httpFactory.CreateClient();

            client.BaseAddress = new Uri(baseUrl);
            contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var userModel = new LoginCommand { Email = "admin@eplatform.vn", Password = "Abc@123$" };

            string stringData = JsonConvert.SerializeObject(userModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            response = await client.PostAsync("/api/v1/AdminUser/Authenticate", contentData);

            if (response.IsSuccessStatusCode)
            {
                apiResult = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Result: {apiResult} at {dateTime}", apiResult, DateTime.UtcNow);

                var responseDto = JsonConvert.DeserializeObject<TokenAdminUserModel>(apiResult);

                Token = responseDto.JwtToken;
                RefreshToken = responseDto.RefreshToken;

                //Authorization: Bearer JWT
                client = _httpFactory.CreateClient();
                client.BaseAddress = new Uri(baseUrl);
                contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                response = await client.GetAsync("/api/v1/StoreManagement/Stores");

                if (response.IsSuccessStatusCode)
                {
                    apiResult = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<PagedApiResult<StoreListResponse>>(apiResult);

                    _logger.LogInformation("Result: {apiResult} at {dateTime}", apiResult, DateTime.UtcNow);

                    _business.PerformBusiness();

                    _logger.LogInformation("{applicationName} {applicationEvent} at {dateTime}", nameof(MyConsoleApplication), "Ended", DateTime.UtcNow);
                }
                else
                {
                    _logger.LogInformation("StatusCode: {apiResult} at {dateTime}", response.StatusCode, DateTime.UtcNow);
                }
            }
            else
            {
                _logger.LogInformation("StatusCode: {apiResult} at {dateTime}", response.StatusCode, DateTime.UtcNow);
            }
        }

        public async Task ShopApi_StoreUser_Management()
        {
            HttpClient client;
            MediaTypeWithQualityHeaderValue contentType;
            HttpResponseMessage response;
            string baseUrl = "https://localhost:44388";
            string apiResult = "";

            _logger.LogInformation("{applicationName} {applicationEvent} at {dateTime}", nameof(MyConsoleApplication), "Started", DateTime.UtcNow);

            //Authentication: Get JWT
            client = _httpFactory.CreateClient();

            client.BaseAddress = new Uri(baseUrl);
            contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            var userModel = new LoginCommand { Email = "storemanager@eplatform.vn", Password = "Abc@123$" };

            string stringData = JsonConvert.SerializeObject(userModel);
            var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

            response = await client.PostAsync("/api/v1/StoreUser/Authenticate", contentData);

            if (response.IsSuccessStatusCode)
            {
                apiResult = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Result: {apiResult} at {dateTime}", apiResult, DateTime.UtcNow);

                var responseDto = JsonConvert.DeserializeObject<TokenAdminUserModel>(apiResult);

                Token = responseDto.JwtToken;
                RefreshToken = responseDto.RefreshToken;

                //Authorization: Bearer JWT
                client = _httpFactory.CreateClient();
                client.BaseAddress = new Uri(baseUrl);
                contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                response = await client.GetAsync("/api/v1/StoreUser/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    apiResult = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<PagedApiResult<StoreListResponse>>(apiResult);

                    _logger.LogInformation("Result: {apiResult} at {dateTime}", apiResult, DateTime.UtcNow);

                    _business.PerformBusiness();

                    _logger.LogInformation("{applicationName} {applicationEvent} at {dateTime}", nameof(MyConsoleApplication), "Ended", DateTime.UtcNow);
                }
                else
                {
                    _logger.LogInformation("StatusCode: {apiResult} at {dateTime}", response.StatusCode, DateTime.UtcNow);
                }
            }
            else
            {
                _logger.LogInformation("StatusCode: {apiResult} at {dateTime}", response.StatusCode, DateTime.UtcNow);
            }
        }
    }
}