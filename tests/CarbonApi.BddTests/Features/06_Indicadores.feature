# language: pt
@indicadores @governanca
Funcionalidade: Indicadores de sustentabilidade
  Como auditor ESG
  Quero consultar indicadores consolidados e o impacto das ações de redução
  Para verificar se os números publicados são íntegros e coerentes

  Contexto:
    Dado que estou autenticado como administrador

  @positivo @contrato
  Cenário: Indicadores consolidados são consistentes
    Quando eu envio uma requisição GET para "/api/v1/indicadores-sustentabilidade"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "indicadores.schema.json"
    E o campo "totalEmpresas" da resposta deve ser maior ou igual a 1
    E o saldo líquido deve ser igual ao total emitido menos o total compensado

  @positivo @contrato
  Cenário: Calcular o impacto de mitigação de um agendamento
    Dado que existe uma empresa cadastrada
    E que existe uma emissão registrada para essa empresa
    E que existe um agendamento de redução cadastrado para essa empresa
    Quando eu envio uma requisição GET para "/api/v1/impacto-mitigacao/{agendamentoId}"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "impacto-mitigacao.schema.json"
    E o campo "agendamentoId" da resposta deve ser "{agendamentoId}"
    E o percentual de impacto deve corresponder à meta dividida pelo total emitido

  @negativo @contrato
  Cenário: Impacto de mitigação para agendamento inexistente
    Quando eu envio uma requisição GET para "/api/v1/impacto-mitigacao/999999999"
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
