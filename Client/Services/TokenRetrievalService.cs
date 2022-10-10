using System.Net.Http.Headers;

namespace spaceapi.Services;

public class TokenRetrievalService
{
    private readonly ILogger<TokenRetrievalService> _logger;
    private readonly IConfiguration _configuration;

    public TokenRetrievalService(ILogger<TokenRetrievalService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<string> GetTokenFromFunction(string secretName)
    {
        string token = null;

        string functionUrl = _configuration["AzureFunctionUrl"];
        string endpoint = $"?key={secretName}";

        HttpClient client = new HttpClient();

        try
        {
            client.BaseAddress = new Uri(functionUrl + "/GetAPIKeyFromStorage");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/text"));
        }

        catch (Exception ex)
        {
            _logger.LogError("Error getting response from API", ex.Message);
        }

        HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            token = await response.Content.ReadAsStringAsync();
        }

        else
        {
            _logger.LogError("An error has occurred", response.StatusCode);
        }

        return token;
    }
}