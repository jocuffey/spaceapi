using System.Net.Http.Headers;
using System.Text.Json;
using spaceapi.Data;

namespace spaceapi.Services;

public class SpaceImageService
{
    private readonly ILogger<SpaceImageService> _logger;
    private readonly IConfiguration _configuration;
    private readonly TokenRetrievalService _tokenRetrievalService;

    public SpaceImageService(ILogger<SpaceImageService> logger, IConfiguration configuration, TokenRetrievalService tokenRetrievalService)
    {
        _logger = logger;
        _configuration = configuration;
        _tokenRetrievalService = tokenRetrievalService;
    }

    public async Task<SpaceImage> GetImageOfTheDay()
    {
        SpaceImage img = new SpaceImage();

        string apiUrl = _configuration["NasaApiUrl"];
        string endpoint = null;

        HttpClient client = new HttpClient();

        try
        {
            string accessKey = await _tokenRetrievalService.GetTokenFromFunction(_configuration["NasaApiKeyName"]);
            endpoint = $"?api_key={accessKey}";

            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
        }

        catch (Exception ex)
        {
            _logger.LogError("Error getting response from API", ex.Message);
        }

        HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var resp = await response.Content.ReadAsStringAsync();
            img = JsonSerializer.Deserialize<SpaceImage>(resp);
        }

        else
        {
            _logger.LogError("An error has occurred", response.StatusCode);
            img.Error = response.StatusCode.ToString();
        }

        return img;
    }
}