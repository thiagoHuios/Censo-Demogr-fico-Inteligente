using IbgeDataInsights.Models;
using IbgeDataInsights.Services;

namespace IbgeDataInsights;

public partial class MainPage : ContentPage
{
    private readonly IbgeService _ibgeService;
    private NomesLocalidadeResultado _resultadoAtual = new();
    private string _categoriaSelecionada = "Homens";
    private bool _carregandoMunicipios;

    public MainPage() : this(new IbgeService())
    {
    }

    public MainPage(IbgeService ibgeService)
    {
        InitializeComponent();
        _ibgeService = ibgeService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (EstadoPicker.ItemsSource is null)
            await CarregarEstadosAsync();
    }

    private async Task CarregarEstadosAsync()
    {
        try
        {
            SetLoading(true);
            var estados = await _ibgeService.ObterEstadosAsync();
            EstadoPicker.ItemsSource = estados;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os estados: {ex.Message}", "OK");
        }
        finally
        {
            SetLoading(false);
        }
    }

    private async void OnEstadoSelected(object sender, EventArgs e)
    {
        if (EstadoPicker.SelectedItem is not Estado estadoSelecionado)
            return;

        try
        {
            SetLoading(true);
            _carregandoMunicipios = true;

            MunicipioPicker.SelectedItem = null;
            MunicipioPicker.IsEnabled = false;

            // Consulta em paralelo: municípios do estado + nomes do estado
            var municipiosTask = _ibgeService.ObterMunicipiosPorEstadoAsync(estadoSelecionado.Id);
            var nomesTask = _ibgeService.ObterNomesPorLocalidadeAsync(estadoSelecionado.Id);

            await Task.WhenAll(municipiosTask, nomesTask);

            MunicipioPicker.ItemsSource = municipiosTask.Result;
            MunicipioPicker.IsEnabled = true;

            _resultadoAtual = nomesTask.Result;
            AtualizarListaExibida();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao consultar dados do estado: {ex.Message}", "OK");
        }
        finally
        {
            _carregandoMunicipios = false;
            SetLoading(false);
        }
    }

    private async void OnMunicipioSelected(object sender, EventArgs e)
    {
        if (_carregandoMunicipios || MunicipioPicker.SelectedItem is not Municipio municipioSelecionado)
            return;

        try
        {
            SetLoading(true);
            _resultadoAtual = await _ibgeService.ObterNomesPorLocalidadeAsync(municipioSelecionado.Id);
            AtualizarListaExibida();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao consultar dados do município: {ex.Message}", "OK");
        }
        finally
        {
            SetLoading(false);
        }
    }

    private void OnCategoriaClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        _categoriaSelecionada = btn.Text;
        AtualizarEstiloBotoes();
        AtualizarListaExibida();
    }

    private void AtualizarEstiloBotoes()
    {
        var corAtiva = Color.FromArgb("#1A365D");
        var corInativa = Color.FromArgb("#CBD5E0");
        var textoAtivo = Colors.White;
        var textoInativo = Color.FromArgb("#1A202C");

        BtnHomens.BackgroundColor = _categoriaSelecionada == "Homens" ? corAtiva : corInativa;
        BtnHomens.TextColor = _categoriaSelecionada == "Homens" ? textoAtivo : textoInativo;

        BtnMulheres.BackgroundColor = _categoriaSelecionada == "Mulheres" ? corAtiva : corInativa;
        BtnMulheres.TextColor = _categoriaSelecionada == "Mulheres" ? textoAtivo : textoInativo;

        BtnSobrenomes.BackgroundColor = _categoriaSelecionada == "Sobrenomes" ? corAtiva : corInativa;
        BtnSobrenomes.TextColor = _categoriaSelecionada == "Sobrenomes" ? textoAtivo : textoInativo;
    }

    private void AtualizarListaExibida()
    {
        NomesCollectionView.ItemsSource = _categoriaSelecionada switch
        {
            "Mulheres" => _resultadoAtual.Mulheres,
            "Sobrenomes" => _resultadoAtual.Sobrenomes,
            _ => _resultadoAtual.Homens
        };
    }

    private void SetLoading(bool isLoading)
    {
        LoadingIndicator.IsVisible = isLoading;
        LoadingIndicator.IsRunning = isLoading;
    }
}