using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CarbonApi.Exceptions;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services.Interfaces;
using CarbonApi.ViewModels.Request;
using CarbonApi.ViewModels.Response;
using Microsoft.IdentityModel.Tokens;

namespace CarbonApi.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponse> AutenticarAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.BuscarPorEmailAsync(request.Email)
            ?? throw new BusinessException("Email ou senha inválidos");

        if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new BusinessException("Email ou senha inválidos");

        if (!usuario.Ativo)
            throw new BusinessException("Usuário inativo");

        var token = GerarToken(usuario.Email, usuario.Role);

        return new LoginResponse
        {
            Token = token,
            Tipo = "Bearer",
            Email = usuario.Email,
            Role = usuario.Role
        };
    }

    private string GerarToken(string email, string role)
    {
        var secret = _configuration["Jwt:Secret"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiration = int.Parse(_configuration["Jwt:ExpirationHours"]!);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}