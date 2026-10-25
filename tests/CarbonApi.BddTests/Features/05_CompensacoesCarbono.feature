# language: pt
@compensacoes
Funcionalidade: Compensação de carbono
  Como gestor ambiental
  Quero registrar ações de compensação, como reflorestamento e créditos de carbono
  Para abater as emissões que não puderam ser evitadas

  Contexto:
    Dado que estou autenticado como administrador
    E que existe uma empresa cadastrada

  @positivo @contrato
  Cenário: Registrar uma compensação por reflorestamento
    Quando eu envio uma requisição POST para "/api/v1/compensacao-carbono" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "tipo": 2,
        "quantidadeTco2e": 30,
        "descricao": "Plantio de 1000 árvores nativas no Cerrado",
        "certificadoUrl": "https://certificados.esg.com.br/ref-2026-001",
        "dataCompensacao": "{ontem}"
      }
      """
    Então o status da resposta deve ser 201
    E a resposta deve respeitar o contrato "compensacao.schema.json"
    E o campo "tipo" da resposta deve ser "2"
    E o campo "empresaId" da resposta deve ser "{empresaId}"

  @positivo @contrato
  Cenário: Listar compensações com paginação
    Quando eu envio uma requisição GET para "/api/v1/compensacao-carbono?page=1&pageSize=10"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "compensacao-lista.schema.json"

  @negativo @contrato
  Cenário: Rejeitar compensação com quantidade zerada
    Quando eu envio uma requisição POST para "/api/v1/compensacao-carbono" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "tipo": 1,
        "quantidadeTco2e": 0,
        "dataCompensacao": "{ontem}"
      }
      """
    Então o status da resposta deve ser 422
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "maior que zero"

  @negativo @contrato
  Cenário: Rejeitar compensação para empresa inexistente
    Quando eu envio uma requisição POST para "/api/v1/compensacao-carbono" com o corpo:
      """
      {
        "empresaId": 999999999,
        "tipo": 3,
        "quantidadeTco2e": 5,
        "dataCompensacao": "{ontem}"
      }
      """
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
