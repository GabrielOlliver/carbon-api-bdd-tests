# language: pt
@emissoes
Funcionalidade: Registro e rastreabilidade de emissões de carbono
  Como responsável pelo inventário de GEE da empresa
  Quero registrar e consultar as emissões de carbono
  Para manter um histórico rastreável e confiável para auditorias ambientais

  Contexto:
    Dado que estou autenticado como administrador
    E que existe uma empresa cadastrada

  @positivo @contrato
  Cenário: Registrar uma emissão de carbono
    Quando eu envio uma requisição POST para "/api/v1/relatorio-emissao" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "quantidadeTco2e": 150.5,
        "escopo": 1,
        "descricao": "Emissões diretas da frota de veículos",
        "dataEmissao": "{ontem}"
      }
      """
    Então o status da resposta deve ser 201
    E a resposta deve respeitar o contrato "emissao.schema.json"
    E o campo "empresaId" da resposta deve ser "{empresaId}"
    E o campo "quantidadeTco2e" da resposta deve ser "150.5"
    E o campo "escopo" da resposta deve ser "1"

  @positivo @contrato
  Cenário: Listar emissões com paginação
    Dado que existe uma emissão registrada para essa empresa
    Quando eu envio uma requisição GET para "/api/v1/emissoes-carbono?page=1&pageSize=10"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "emissao-lista.schema.json"
    E a lista "data" da resposta não deve estar vazia

  @positivo @contrato @governanca
  Cenário: Histórico de emissões contém somente registros da empresa consultada
    Dado que existe uma emissão registrada para essa empresa
    Quando eu envio uma requisição GET para "/api/v1/historico-emissoes/{empresaId}?page=1&pageSize=10"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "emissao-lista.schema.json"
    E todos os itens da lista "data" devem ter o campo "empresaId" igual a "{empresaId}"

  @negativo @contrato @governanca
  Esquema do Cenário: Rejeitar registro de emissão inválido
    Quando eu envio uma requisição POST para "/api/v1/relatorio-emissao" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "quantidadeTco2e": <quantidade>,
        "escopo": 1,
        "descricao": "Registro inválido",
        "dataEmissao": "<data>"
      }
      """
    Então o status da resposta deve ser 422
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "<mensagem>"

    Exemplos:
      | regra                     | quantidade | data     | mensagem            |
      | quantidade zero           | 0          | {ontem}  | maior que zero      |
      | quantidade negativa       | -10.5      | {ontem}  | maior que zero      |
      | data de emissão no futuro | 50         | {amanha} | não pode ser futura |

  @negativo @contrato
  Cenário: Registrar emissão para empresa inexistente
    Quando eu envio uma requisição POST para "/api/v1/relatorio-emissao" com o corpo:
      """
      {
        "empresaId": 999999999,
        "quantidadeTco2e": 10,
        "escopo": 2,
        "descricao": "Empresa inexistente",
        "dataEmissao": "{ontem}"
      }
      """
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "Empresa não encontrada"

  @negativo @contrato
  Cenário: Consultar histórico de empresa inexistente
    Quando eu envio uma requisição GET para "/api/v1/historico-emissoes/999999999"
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
