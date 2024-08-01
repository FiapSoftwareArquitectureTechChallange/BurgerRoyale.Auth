# BurgerRoyale.Auth

Esta é uma API de autenticação desenvolvida em .NET 8 que fornece endpoints para gerenciar contas de usuário e operações relacionadas.

## Tecnologias Utilizadas

- **Linguagem:** C#
- **Framework:** ASP.NET Core
- **Banco de Dados:** SQL Server

## Configuração do Ambiente

Para configurar o ambiente de desenvolvimento, siga os passos abaixo:

1. **Clone o Repositório:**

   ```bash
   git clone https://github.com/FiapSoftwareArquitectureTechChallange/BurgerRoyale.Auth.git
   ```

2. **Navegue até o Diretório do Projeto:**

   ```bash
   cd BurgerRoyale.Auth
   ```

3. **Restaurar Dependências:**

   Utilize o `dotnet` CLI para restaurar as dependências do projeto:

   ```bash
   dotnet restore
   ```

4. **Configurar Variáveis de Ambiente:**

   Crie um arquivo `appsettings.Development.json` na raiz do projeto e adicione as variáveis de ambiente necessárias. Consulte o arquivo `appsettings.json` para ver exemplos de configuração.

## Uso

Para iniciar o serviço, use o seguinte comando:

```bash
dotnet run
```

O serviço estará disponível em `http://localhost:5000` (ou a porta configurada).
