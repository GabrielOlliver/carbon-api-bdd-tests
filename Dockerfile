# =====================================================================
# ESTAGIO 1 - BUILD: imagem com o SDK completo (~800 MB), so para compilar
# =====================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) Copia SOMENTE os arquivos de dependencias e faz o restore.
#    Enquanto o .csproj nao mudar, o Docker reaproveita esta camada do cache
#    e o restore (lento) nao roda de novo a cada alteracao de codigo.
COPY global.json ./
COPY src/CarbonApi/CarbonApi.csproj src/CarbonApi/
RUN dotnet restore src/CarbonApi/CarbonApi.csproj

# 2) Agora copia o codigo-fonte e publica em modo Release.
COPY src/ src/
RUN dotnet publish src/CarbonApi/CarbonApi.csproj \
        -c Release \
        -o /app/publish \
        --no-restore \
        /p:UseAppHost=false

# =====================================================================
# ESTAGIO 2 - RUNTIME: imagem enxuta, so com o runtime do ASP.NET
# =====================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# curl e usado pelo HEALTHCHECK (a imagem base nao traz nenhum cliente HTTP)
RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && rm -rf /var/lib/apt/lists/*

# Copia apenas o resultado do publish - o SDK e o codigo-fonte ficam para tras
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Roda como usuario sem privilegios (o usuario "app" ja existe na imagem base)
USER $APP_UID

# O Docker marca o container como healthy/unhealthy consultando a API
HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
  CMD curl -fsS http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CarbonApi.dll"]
