using System.Text.Json.Serialization;

namespace IbgeDataInsights.Models;

public class Municipio
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;
}