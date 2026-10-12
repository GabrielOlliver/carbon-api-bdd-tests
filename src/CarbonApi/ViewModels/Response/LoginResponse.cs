namespace CarbonApi.ViewModels.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string Tipo { get; set; } = "Bearer";
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}