#!/usr/bin/env bash
# Executa a suite BDD localmente:
#   1. sobe API + Oracle com docker compose
#   2. espera a API ficar pronta
#   3. roda os cenarios Gherkin (Reqnroll)
#   4. mostra onde estao os relatorios
set -euo pipefail
cd "$(dirname "$0")/.."

if [ ! -f .env ]; then
  echo "Arquivo .env nao encontrado. Crie com: cp .env.example .env (e ajuste as senhas)"; exit 1
fi

echo ">> Subindo a stack (API + Oracle)..."
docker compose up -d --build

echo ">> Aguardando a API em http://localhost:8080/health/ready ..."
for i in $(seq 1 60); do
  if curl -fsS http://localhost:8080/health/ready >/dev/null 2>&1; then echo "   API pronta."; break; fi
  [ "$i" -eq 60 ] && { echo "API nao ficou pronta. Veja: docker compose logs carbon-api"; exit 1; }
  sleep 5
done

echo ">> Executando os cenarios BDD..."
dotnet test tests/CarbonApi.BddTests/CarbonApi.BddTests.csproj \
  --logger "console;verbosity=normal" \
  --logger "trx;LogFileName=bdd-tests.trx" \
  --results-directory TestResults

echo ""
echo ">> Relatorio HTML: tests/CarbonApi.BddTests/bin/Debug/net8.0/reqnroll_report.html"
echo ">> Relatorio TRX : TestResults/bdd-tests.trx"
