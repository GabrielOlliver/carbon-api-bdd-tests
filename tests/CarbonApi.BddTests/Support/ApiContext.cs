using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace CarbonApi.BddTests.Support;

/// <summary>
/// Estado compartilhado entre os steps de UM cenario.
/// O Reqnroll cria uma instancia nova por cenario e a injeta no construtor
/// das classes de steps (context injection) - cenarios nao interferem entre si.
/// </summary>
public class ApiContext
{
    /// URL da API sob teste. Padrao: stack local do docker compose.
    public static readonly string BaseUrl =
        (Environment.GetEnvironmentVariable("API_BASE_URL") ?? "http://localhost:8080").TrimEnd('/');

    // HttpClient e thread-safe e deve ser reutilizado (evita esgotar sockets)
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(60) };

    public string? Token { get; set; }
    public HttpResponseMessage? Response { get; private set; }
    public string ResponseBody { get; private set; } = string.Empty;

    /// Variaveis usadas como {placeholder} nos caminhos e corpos dos cenarios.
    public Dictionary<string, string> Variaveis { get; } = new();

    public ApiContext()
    {
        var agora = DateTime.UtcNow;
        const string formato = "yyyy-MM-ddTHH:mm:ss";
        Variaveis["cnpjUnico"] = GerarCnpj();               // dado unico a cada execucao
        Variaveis["sufixo"] = Guid.NewGuid().ToString("N")[..6];
        Variaveis["ontem"] = agora.AddDays(-1).ToString(formato);
        Variaveis["hoje"] = agora.Date.ToString(formato);
        Variaveis["amanha"] = agora.AddDays(1).ToString(formato);
        Variaveis["daqui30dias"] = agora.Date.AddDays(30).ToString(formato);
        Variaveis["daqui90dias"] = agora.Date.AddDays(90).ToString(formato);
    }

    public static string GerarCnpj()
    {
        var r = Random.Shared;
        return $"{r.Next(10, 100)}.{r.Next(100, 1000)}.{r.Next(100, 1000)}/0001-{r.Next(10, 100)}";
    }

    /// Troca {variavel} pelo valor salvo no cenario.
    public string Resolver(string texto)
    {
        foreach (var (chave, valor) in Variaveis)
            texto = texto.Replace("{" + chave + "}", valor);
        return texto;
    }

    public async Task EnviarAsync(HttpMethod metodo, string caminho, string? corpo = null)
    {
        using var request = new HttpRequestMessage(metodo, BaseUrl + Resolver(caminho));

        if (!string.IsNullOrEmpty(Token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

        if (corpo is not null)
            request.Content = new StringContent(Resolver(corpo), Encoding.UTF8, "application/json");

        Response = await Http.SendAsync(request);
        ResponseBody = await Response.Content.ReadAsStringAsync();
    }

    public JsonNode Json =>
        JsonNode.Parse(ResponseBody) ?? throw new InvalidOperationException("A resposta nao possui corpo JSON.");

    /// Le um campo do JSON por caminho simples: "nome", "data[0].empresaId".
    public JsonNode? Campo(string caminho)
    {
        JsonNode? atual = Json;
        foreach (var parte in caminho.Split('.'))
        {
            var nome = parte;
            int? indice = null;
            var abre = parte.IndexOf('[');
            if (abre >= 0)
            {
                nome = parte[..abre];
                indice = int.Parse(parte[(abre + 1)..parte.IndexOf(']')]);
            }
            if (nome.Length > 0) atual = atual?[nome];
            if (indice is not null) atual = atual?[indice.Value];
        }
        return atual;
    }

    public static string ComoTexto(JsonNode? no) => no switch
    {
        null => "null",
        JsonValue v when v.TryGetValue<string>(out var s) => s,
        _ => no.ToJsonString()
    };
}
