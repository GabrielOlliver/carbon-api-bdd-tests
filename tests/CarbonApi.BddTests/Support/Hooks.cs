using Reqnroll;

namespace CarbonApi.BddTests.Support;

[Binding]
public class Hooks
{
    /// Antes de toda a suite: garante que a API (e o banco) estao no ar.
    /// Falha cedo com uma mensagem clara, em vez de dezenas de erros de conexao.
    [BeforeTestRun]
    public static async Task VerificarApiNoAr()
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
        for (var tentativa = 1; tentativa <= 30; tentativa++)
        {
            try
            {
                var resposta = await http.GetAsync(ApiContext.BaseUrl + "/health/ready");
                if (resposta.IsSuccessStatusCode) return;
            }
            catch (HttpRequestException) { }
            catch (TaskCanceledException) { }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        throw new InvalidOperationException(
            $"A API nao respondeu em {ApiContext.BaseUrl}/health/ready. " +
            "Suba a stack com 'docker compose up -d' antes de executar os testes.");
    }
}
