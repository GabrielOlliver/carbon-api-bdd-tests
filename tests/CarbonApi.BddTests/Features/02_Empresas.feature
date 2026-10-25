# language: pt
@empresas
Funcionalidade: Gestão de empresas sustentáveis
  Como administrador da plataforma ESG
  Quero cadastrar e consultar empresas
  Para manter um cadastro íntegro, sem duplicidade de CNPJ

  Contexto:
    Dado que estou autenticado como administrador

  @positivo @contrato
  Cenário: Cadastrar uma nova empresa
    Quando eu envio uma requisição POST para "/api/v1/empresas-sustentaveis" com o corpo:
      """
      {
        "nome": "Solar Verde Ltda",
        "cnpj": "{cnpjUnico}",
        "setor": "Energia Solar",
        "pais": "Brasil"
      }
      """
    Então o status da resposta deve ser 201
    E a resposta deve respeitar o contrato "empresa.schema.json"
    E o campo "nome" da resposta deve ser "Solar Verde Ltda"
    E o campo "cnpj" da resposta deve ser "{cnpjUnico}"
    E o campo "ativa" da resposta deve ser "true"

  @positivo @contrato
  Cenário: Listar empresas com paginação
    Quando eu envio uma requisição GET para "/api/v1/empresas-sustentaveis?page=1&pageSize=5"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "empresa-lista.schema.json"
    E o campo "pageNumber" da resposta deve ser "1"
    E o campo "pageSize" da resposta deve ser "5"
    E a lista "data" da resposta não deve estar vazia

  @positivo @contrato
  Cenário: Consultar uma empresa pelo id
    Dado que existe uma empresa cadastrada
    Quando eu envio uma requisição GET para "/api/v1/empresas-sustentaveis/{empresaId}"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "empresa.schema.json"
    E o campo "id" da resposta deve ser "{empresaId}"
    E o campo "cnpj" da resposta deve ser "{empresaCnpj}"

  @negativo @contrato @governanca
  Cenário: Não permitir duas empresas com o mesmo CNPJ
    Dado que existe uma empresa cadastrada
    Quando eu envio uma requisição POST para "/api/v1/empresas-sustentaveis" com o corpo:
      """
      {
        "nome": "Empresa Duplicada",
        "cnpj": "{empresaCnpj}",
        "setor": "Varejo",
        "pais": "Brasil"
      }
      """
    Então o status da resposta deve ser 409
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "já cadastrada"

  @negativo @contrato
  Cenário: Consultar uma empresa inexistente
    Quando eu envio uma requisição GET para "/api/v1/empresas-sustentaveis/999999999"
    Então o status da resposta deve ser 404
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "Empresa não encontrada"

  @positivo @contrato
  Cenário: Consultar a empresa cadastrada pelo seed inicial
    Quando eu envio uma requisição GET para "/api/v1/empresas-sustentaveis/1"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "empresa.schema.json"
    E o campo "nome" da resposta deve ser "EcoTech Brasil"
    E o campo "cnpj" da resposta deve ser "12.345.678/0001-99"
