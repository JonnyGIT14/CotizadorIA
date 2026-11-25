using System.Net.Http.Json;
using System.Text.Json;

namespace CotizadorIA.Services;

public sealed class FrankfurterExchangeRateService(HttpClient httpClient)
{
    public async Task<ExchangeRateQuote> GetLatestUsdToMxnAsync(CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(
            "v1/latest?base=USD&symbols=MXN",
            cancellationToken);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<FrankfurterResponse>(cancellationToken)
            ?? throw new JsonException("La respuesta de Frankfurter está vacía.");

        if (result.Date == default
            || result.Rates is null
            || !result.Rates.TryGetValue("MXN", out var rate)
            || rate <= 0)
        {
            throw new JsonException("La respuesta de Frankfurter no contiene fecha y tasa USD/MXN válidas.");
        }

        return new ExchangeRateQuote(rate, result.Date);
    }

    private sealed class FrankfurterResponse
    {
        public DateOnly Date { get; set; }

        public Dictionary<string, decimal>? Rates { get; set; }
    }
}

public sealed record ExchangeRateQuote(decimal Rate, DateOnly Date);
