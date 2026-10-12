using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> AutenticarAsync(LoginRequest request);
}