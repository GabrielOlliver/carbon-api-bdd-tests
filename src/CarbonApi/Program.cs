using System.Text;
using CarbonApi.Data;
using CarbonApi.Middleware;
using CarbonApi.Repositories;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services;
using CarbonApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== DbContext Oracle =====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// ===== Repositories =====
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IEmissaoCarbonoRepository, EmissaoCarbonoRepository>();
builder.Services.AddScoped<IAgendamentoReducaoRepository, AgendamentoReducaoRepository>();
builder.Services.AddScoped<ICompensacaoCarbonoRepository, CompensacaoCarbonoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// ===== Services =====
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IEmissaoCarbonoService, EmissaoCarbonoService>();
builder.Services.AddScoped<IAgendamentoReducaoService, AgendamentoReducaoService>();
builder.Services.AddScoped<ICompensacaoCarbonoService, CompensacaoCarbonoService>();
builder.Services.AddScoped<IIndicadoresService, IndicadoresService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ===== JWT Authentication =====
var jwtSecret = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(jwtSecret) || jwtSecret.Length < 32)
    throw new InvalidOperationException(
        "Jwt:Secret não configurado. Defina a variável de ambiente Jwt__Secret (mínimo 32 caracteres).");
var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ===== Controllers =====
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ===== Swagger com JWT =====
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Carbon API — ESG",
        Version = "v1",
        Description = "API RESTful para Gestão de Sustentabilidade e Emissões de Carbono"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Ex: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ===== Middleware =====
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();