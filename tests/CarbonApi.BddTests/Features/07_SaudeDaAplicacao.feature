# language: pt
@saude
Funcionalidade: Saúde da aplicação
  Como equipe de operações
  Quero verificar se a API e o banco de dados estão disponíveis
  Para detectar falhas antes que afetem os usuários

  @positivo
  Cenário: A API está no ar
    Quando eu envio uma requisição GET para "/health"
    Então o status da resposta deve ser 200
    E o corpo da resposta deve ser "Healthy"

  @positivo
  Cenário: A API está conectada ao banco de dados
    Quando eu envio uma requisição GET para "/health/ready"
    Então o status da resposta deve ser 200
    E o corpo da resposta deve ser "Healthy"
