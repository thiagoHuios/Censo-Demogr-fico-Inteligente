using System.Text.Json;
using IbgeDataInsights.Models;

namespace IbgeDataInsights.Services;

public class IbgeService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string BaseLocalidadesUrl = "https://servicodados.ibge.gov.br/api/v1/localidades";
    private const string BaseNomesUrl = "https://servicodados.ibge.gov.br/api/v3/nomes/2022/localidade";

    public IbgeService()
    {
        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<List<Estado>> ObterEstadosAsync()
    {
        var url = $"{BaseLocalidadesUrl}/estados?orderBy=nome";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Estado>>(json, _jsonOptions) ?? new List<Estado>();
    }

    public async Task<List<Municipio>> ObterMunicipiosPorEstadoAsync(int estadoId)
    {
        var url = $"{BaseLocalidadesUrl}/estados/{estadoId}/municipios?orderBy=nome";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Municipio>>(json, _jsonOptions) ?? new List<Municipio>();
    }

    public async Task<NomesLocalidadeResultado> ObterNomesPorLocalidadeAsync(int localidadeId)
    {
        var url = $"{BaseNomesUrl}/{localidadeId}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var resultado = new NomesLocalidadeResultado();

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        if (root.ValueKind == JsonValueKind.Object)
        {
            resultado.Homens = ExtrairLista(root, "masculino", "homens", "nomesMasculinos");
            resultado.Mulheres = ExtrairLista(root, "feminino", "mulheres", "nomesFemininos");
            resultado.Sobrenomes = ExtrairLista(root, "sobrenomes", "sobrenome");
        }

        return resultado;
    }

    private List<NomeItem> ExtrairLista(JsonElement root, params string[] possiveisChaves)
    {
        foreach (var chave in possiveisChaves)
        {
            if (root.TryGetProperty(chave, out var elemento) && elemento.ValueKind == JsonValueKind.Array)
            {
                var lista = JsonSerializer.Deserialize<List<NomeItem>>(elemento.GetRawText(), _jsonOptions);
                if (lista is not null)
                {
                    var ordenada = lista.OrderByDescending(x => x.Frequencia).ToList();
                    for (int i = 0; i < ordenada.Count; i++)
                    {
                        if (ordenada[i].Ranking == 0)
                            ordenada[i].Ranking = i + 1;
                    }
                    return ordenada;
                }
            }
        }
        return new List<NomeItem>();
    }
}