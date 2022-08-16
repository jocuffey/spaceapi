using System.Text.Json.Serialization;

namespace spaceapi.Data;

[Serializable]
public class SpaceImage
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }
}
