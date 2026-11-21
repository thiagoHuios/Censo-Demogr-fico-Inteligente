using System.Text.Json.Serialization;

namespace IbgeDataInsights.Models;

public class NomeItem
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("frequencia")]
    public int Frequencia { get; set; }

    [JsonPropertyName("ranking")]
    public int Ranking { get; set; }

    public string NomeFormatado => string.IsNullOrWhiteSpace(Nome)
        ? string.Empty
        : char.ToUpper(Nome[0]) + Nome[1..].ToLower();

    public string RankingFormatado => $"#{Ranking}";
    public string FrequenciaFormatada => $"{Frequencia:N0} pessoas";
}

public class NomesLocalidadeResultado
{
    public List<NomeItem> Homens { get; set; } = new();
    public List<NomeItem> Mulheres { get; set; } = new();
    public List<NomeItem> Sobrenomes { get; set; } = new();
}