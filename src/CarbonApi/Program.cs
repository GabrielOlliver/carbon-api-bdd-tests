using System.Text;
using CarbonApi.Data;
using CarbonApi.Middleware;
using CarbonApi.Repositories;
using CarbonApi.Repositories.Interfaces;
using CarbonApi.Services;
using CarbonApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== DbContext Oracle =====
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// ===== Health Checks =====
// /health       -> liveness: o processo esta de pe?
// /health/ready -> readiness: a API consegue falar com o banco?
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database", tags: new[] { "ready" });

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

// ===== Migrations automaticas =====
// Cria/atualiza as tabelas (e o seed do usuario admin) na inicializacao.
// O retry cobre o caso do Oracle ainda estar terminando de subir.
if (app.Configuration.GetValue("Database:ApplyMigrations", true))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    const int maxTentativas = 10;
    for (var tentativa = 1; ; tentativa++)
    {
        try
        {
            db.Database.Migrate();

            // O seed (HasData) insere Empresa e Usuario com Id = 1 explicito, mas a
            // coluna IDENTITY do Oracle continua comecando em 1 -> o primeiro INSERT
            // geraria Id duplicado (ORA-00001). "START WITH LIMIT VALUE" reposiciona
            // a identity para MAX(ID) + 1. E idempotente: pode rodar a cada startup.
            // SQL 100% constante (sem interpolacao), sem risco de SQL injection
            db.Database.ExecuteSqlRaw(
                "ALTER TABLE \"TB_EMPRESA\" MODIFY \"ID\" GENERATED BY DEFAULT ON NULL AS IDENTITY (START WITH LIMIT VALUE)");
            db.Database.ExecuteSqlRaw(
                "ALTER TABLE \"TB_USUARIO\" MODIFY \"ID\" GENERATED BY DEFAULT ON NULL AS IDENTITY (START WITH LIMIT VALUE)");

            logger.LogInformation("Migrations aplicadas com sucesso.");
            break;
        }
        catch (Exception ex) when (tentativa < maxTentativas)
        {
            logger.LogWarning(ex, "Banco indisponivel (tentativa {Tentativa}/{Max}). Nova tentativa em 5s...", tentativa, maxTentativas);
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}

// ===== Middleware =====
app.UseMiddleware<ExceptionMiddleware>();

// Swagger habilitado em todos os ambientes (Development, Staging, Production)
// para permitir a validacao visual dos deploys.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => false });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = c => c.Tags.Contains("ready") });
app.Run();