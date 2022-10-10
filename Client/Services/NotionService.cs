using spaceapi.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace spaceapi.Services
{
    public class NotionService
    {
        private readonly ILogger<NotionService> _logger;
        private readonly IConfiguration _configuration;

        public NotionService(ILogger<NotionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<List<NotionUser>> GetAllUsers()
        {
            var users = new List<NotionUser>();

            string? apiUrl = _configuration["AzureFunctionUrl"];

            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync("/api/GetNotionContent").ConfigureAwait(false);

            }

            catch (Exception ex)
            {
                _logger.LogError("Error getting response from API", ex.Message);
            }


            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                users = JsonSerializer.Deserialize<List<NotionUser>>(resp);
            }

            else
            {
                _logger.LogError("An error has occurred", response.StatusCode);
            }

            return users;
        }
    }
}
