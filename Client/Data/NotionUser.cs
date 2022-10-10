using System.Text.Json.Serialization;

namespace spaceapi.Data;

[Serializable]
public class NotionUser : ApiResponse
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("avatar_url")]
    public string? Avatar { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
