# language: pt
@agendamentos
Funcionalidade: Agendamentos de redução de emissões
  Como gestor ambiental
  Quero planejar, acompanhar e cancelar iniciativas de redução de carbono
  Para cumprir as metas de descarbonização da empresa

  Contexto:
    Dado que estou autenticado como administrador
    E que existe uma empresa cadastrada

  @positivo @contrato
  Cenário: Criar um agendamento de redução
    Quando eu envio uma requisição POST para "/api/v1/agendamento-reducao-carbono" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "titulo": "Substituição de frota diesel por elétrica",
        "descricao": "Substituir 30% da frota até o fim do ano",
        "metaReducaoTco2e": 75.25,
        "dataInicio": "{hoje}",
        "dataFimPrevista": "{daqui90dias}"
      }
      """
    Então o status da resposta deve ser 201
    E a resposta deve respeitar o contrato "agendamento.schema.json"
    E o campo "status" da resposta deve ser "1"
    E o campo "metaReducaoTco2e" da resposta deve ser "75.25"

  @positivo @contrato
  Cenário: Listar agendamentos com paginação
    Dado que existe um agendamento de redução cadastrado para essa empresa
    Quando eu envio uma requisição GET para "/api/v1/agendamento-reducao-carbono?page=1&pageSize=10"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "agendamento-lista.schema.json"
    E a lista "data" da resposta não deve estar vazia

  @positivo @contrato
  Cenário: Consultar um agendamento pelo id
    Dado que existe um agendamento de redução cadastrado para essa empresa
    Quando eu envio uma requisição GET para "/api/v1/agendamento-reducao-carbono/{agendamentoId}"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "agendamento.schema.json"
    E o campo "id" da resposta deve ser "{agendamentoId}"
    E o campo "empresaId" da resposta deve ser "{empresaId}"

  @positivo @contrato
  Cenário: Atualizar o andamento de um agendamento
    Dado que existe um agendamento de redução cadastrado para essa empresa
    Quando eu envio uma requisição PUT para "/api/v1/agendamento-reducao-carbono/{agendamentoId}" com o corpo:
      """
      {
        "titulo": "Substituição de frota - revisado",
        "descricao": "Meta ampliada para 50% da frota",
        "metaReducaoTco2e": 120,
        "status": 2,
        "dataFimPrevista": "{daqui90dias}"
      }
      """
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "agendamento.schema.json"
    E o campo "status" da resposta deve ser "2"
    E o campo "titulo" da resposta deve ser "Substituição de frota - revisado"

  @positivo
  Cenário: Cancelar um agendamento
    Dado que existe um agendamento de redução cadastrado para essa empresa
    Quando eu envio uma requisição DELETE para "/api/v1/agendamento-reducao-carbono/{agendamentoId}"
    Então o status da resposta deve ser 204
    Quando eu envio uma requisição GET para "/api/v1/agendamento-reducao-carbono/{agendamentoId}"
    Então o campo "status" da resposta deve ser "4"

  @negativo @contrato
  Esquema do Cenário: Rejeitar agendamento com dados inválidos
    Quando eu envio uma requisição POST para "/api/v1/agendamento-reducao-carbono" com o corpo:
      """
      {
        "empresaId": {empresaId},
        "titulo": "Agendamento inválido",
        "metaReducaoTco2e": <meta>,
        "dataInicio": "<inicio>",
        "dataFimPrevista": "<fim>"
      }
      """
    Então o status da resposta deve ser 422
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "<mensagem>"

    Exemplos:
      | regra                       | meta | inicio        | fim           | mensagem       |
      | data fim anterior ao início | 50   | {daqui90dias} | {daqui30dias} | posterior      |
      | meta de redução zerada      | 0    | {hoje}        | {daqui90dias} | maior que zero |

  @negativo @contrato @governanca
  Cenário: Não permitir alterar um agendamento cancelado
    Dado que existe um agendamento de redução cadastrado para essa empresa
    E que esse agendamento foi cancelado
    Quando eu envio uma requisição PUT para "/api/v1/agendamento-reducao-carbono/{agendamentoId}" com o corpo:
      """
      {
        "titulo": "Tentativa de reativar",
        "metaReducaoTco2e": 10,
        "status": 2,
        "dataFimPrevista": "{daqui90dias}"
      }
      """
    Então o status da resposta deve ser 422
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "cancelado"

  @negativo @contrato
  Cenário: Não permitir cancelar duas vezes o mesmo agendamento
    Dado que existe um agendamento de redução cadastrado para essa empresa
    E que esse agendamento foi cancelado
    Quando eu envio uma requisição DELETE para "/api/v1/agendamento-reducao-carbono/{agendamentoId}"
    Então o status da resposta deve ser 422
    E a mensagem de erro deve conter "já está cancelado"

  @negativo @contrato
  Cenário: Consultar um agendamento inexistente
    Quando eu envio uma requisição GET para "/api/v1/agendamento-reducao-carbono/999999999"
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
