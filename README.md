# IBGE Data Insights

Aplicação mobile desenvolvida em **.NET MAUI** para consumo encadeado das APIs públicas do IBGE (Localidades e Nomes — Censo 2022), permitindo visualizar os nomes masculinos, nomes femininos e sobrenomes mais frequentes por Estado ou Município.

## Integrantes do Grupo
- Thiago Alexandre Brito | 4º Período ADS
- Bruno Cogo | 4º Período ADS
- Thiago Henrique Silva | 4º Período ADS

## Arquitetura do Projeto
- **Models/**: Mapeamento dos objetos JSON retornados pelas APIs (`Estado`, `Municipio`, `NomeItem`).
- **Services/**: Integração HTTP com `HttpClient` e desserialização via `System.Text.Json` (`IbgeService`).
- **Views (`MainPage`)**: Interface com `Picker` encadeado, filtros por categoria e `CollectionView`.

## APIs Utilizadas
- Localidades (Estados): `https://servicodados.ibge.gov.br/api/v1/localidades/estados`
- Localidades (Municípios): `https://servicodados.ibge.gov.br/api/v1/localidades/estados/{id}/municipios`
- Nomes (Censo 2022): `https://servicodados.ibge.gov.br/api/v3/nomes/2022/localidade/{id_localidade}`

## Como Executar
1. Clone o repositório:
   ```bash
   git clone [https://github.com/seu-usuario/ibge-data-insights.git](https://github.com/seu-usuario/ibge-data-insights.git)