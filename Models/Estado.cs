using System.Text.Json.Serialization;

namespace IbgeDataInsights.Models;

public class Estado
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("sigla")]
    public string Sigla { get; set; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    public string NomeFormatado => $"{Nome} ({Sigla})";
}