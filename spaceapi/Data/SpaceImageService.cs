using System.Net.Http.Headers;
using System.Text.Json;

namespace spaceapi.Data;

public class SpaceImageService
{
    private readonly ILogger<SpaceImageService> _logger;
    private readonly IConfiguration _configuration;

    public SpaceImageService(ILogger<SpaceImageService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<SpaceImage> GetImageOfTheDay()
    {
        SpaceImage img = new SpaceImage();
        string apiUrl = _configuration["NasaApiEndpoint"];
        string accessKey = _configuration["NasaApiKey"];
        string b = $"?api_key={accessKey}";

        HttpClient client = new HttpClient();

        client.BaseAddress = new Uri(apiUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync(b).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var resp = await response.Content.ReadAsStringAsync();
            img = JsonSerializer.Deserialize<SpaceImage>(resp);

        }

        return img;
    }
}