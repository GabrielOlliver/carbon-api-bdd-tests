# ================================
# ESTÁGIO 1: Build
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY ["CarbonApi/CarbonApi.csproj", "CarbonApi/"]
COPY ["CarbonApi.Tests/CarbonApi.Tests.csproj", "CarbonApi.Tests/"]

# Restaura as dependências
RUN dotnet restore "CarbonApi/CarbonApi.csproj"

# Copia todo o código fonte
COPY . .

# Compila e publica
WORKDIR /src/CarbonApi
RUN dotnet publish "CarbonApi.csproj" -c Release -o /app/publish --no-restore

# ================================
# ESTÁGIO 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CarbonApi.dll"]