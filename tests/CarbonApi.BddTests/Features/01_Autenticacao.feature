# language: pt
@autenticacao @governanca
Funcionalidade: Autenticação de usuários
  Como gestor de sustentabilidade de uma empresa
  Quero me autenticar na Carbon API
  Para que somente pessoas autorizadas registrem e consultem dados ambientais

  @positivo @contrato
  Cenário: Login com credenciais válidas retorna um token JWT
    Quando eu faço login com o email "admin@esg.com" e a senha "admin123"
    Então o status da resposta deve ser 200
    E a resposta deve respeitar o contrato "login.schema.json"
    E o campo "tipo" da resposta deve ser "Bearer"
    E o campo "role" da resposta deve ser "ADMIN"
    E o campo "token" da resposta deve estar preenchido

  @negativo @contrato
  Esquema do Cenário: Login com credenciais inválidas é recusado
    Quando eu faço login com o email "<email>" e a senha "<senha>"
    Então o status da resposta deve ser 422
    E a resposta deve respeitar o contrato "erro.schema.json"
    E a mensagem de erro deve conter "Email ou senha inválidos"

    Exemplos:
      | situacao            | email             | senha       |
      | senha incorreta     | admin@esg.com     | senhaErrada |
      | usuário inexistente | ninguem@esg.com   | admin123    |

  @negativo
  Cenário: Acesso a um recurso protegido sem token é bloqueado
    Dado que não estou autenticado
    Quando eu envio uma requisição GET para "/api/v1/empresas-sustentaveis"
    Então o status da resposta deve ser 401

  @negativo
  Cenário: Acesso com token inválido é bloqueado
    Dado que estou autenticado com um token inválido
    Quando eu envio uma requisição GET para "/api/v1/indicadores-sustentabilidade"
    Então o status da resposta deve ser 401
