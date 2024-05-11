# BurgerRoyale Auth
# API de Autenticação em .NET 8

Esta é uma API de autenticação desenvolvida em .NET 8 que fornece endpoints para gerenciar contas de usuário e operações relacionadas.

## Endpoints

### Account

- **POST /api/account/login**
  - Endpoint para login de usuários.
  - Parâmetros:
    - `cpf`: string.
    - `email`: string.
    - `password`: string.
  - Retorna um token de acesso válido se as credenciais forem válidas.

- **POST /api/account/register**
  - Endpoint para registrar novos usuários.
  - Parâmetros:
    - `cpf`: string.
    - `name`: string.
    - `email`: string.
    - `password`: string.
    - `passwordConfirmation`: string.
  - Retorna o usuário criado com seu ID.

- **PUT /api/account/update**
  - Endpoint para atualizar os detalhes da conta do usuário.
  - Requer autenticação.
  - Parâmetros:
    - `name`: string.
    - `email`: string.
    - `currentPassword`: string.
    - `newPassword`: string.
    - `newPasswordConfirmation`: string.
  - Retorna o usuário com seu ID e os novos valores.

### User

- **GET /api/user**
  - Endpoint para obter todos os usuários.
  - Requer autenticação.
  - Parâmetros:
    - `userType`: int.
  - Retorna uma lista de todos os usuários cadastrados baseado no filtro de userType.

- **POST /api/user**
  - Endpoint para adicionar um novo usuário.
  - Requer autenticação.
  - Parâmetros:
    - `cpf`: string.
    - `name`: string.
    - `email`: string.
    - `password`: string.
    - `userRole`: int.
  - Retorna os detalhes do usuário adicionado.

- **GET /api/user/{id}**
  - Endpoint para obter detalhes de um usuário específico pelo ID.
  - Requer autenticação.
  - Parâmetros:
    - `id`: int.
  - Retorna os detalhes do usuário correspondente ao ID fornecido.

- **PUT /api/user/{id}**
  - Endpoint para atualizar os detalhes de um usuário existente.
  - Requer autenticação.
  - Parâmetros:
    - `name`: string.
    - `email`: string.
    - `password`: string.
    - `userRole`: int.
  - Retorna os detalhes atualizados do usuário.

- **DELETE /api/user/{id}**
  - Endpoint para excluir um usuário existente.
  - Requer autenticação.
  - Parâmetros:
    - `id`: int.
  - Retorna status code 204.
