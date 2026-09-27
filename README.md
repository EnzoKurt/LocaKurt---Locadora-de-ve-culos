# Locadora de Veículos

Projeto acadêmico de backend para uma locadora de veículos.

## Tecnologias

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server Express

## Entidades

- Fabricante
- Categoria
- Veículo
- Cliente
- Aluguel
- Pagamento

## Como executar

### 1. Requisitos

Instale:

- .NET 8 SDK
- SQL Server Express
- Visual Studio 2022 ou VS Code com C# Dev Kit

### 2. Configurar o banco

Confira a conexão em:

```text
appsettings.json
```

Por padrão:

```text
Server=.\SQLEXPRESS;Database=LocadoraVeiculos;Trusted_Connection=True;TrustServerCertificate=True
```

Se sua instância do SQL Express tiver outro nome, ajuste o valor.

### 3. Restaurar os pacotes

```bash
dotnet restore
```

### 4. Criar/executar

```bash
dotnet run
```

A aplicação cria o banco caso ele ainda não exista.

### 5. Rotas

CRUD:

```text
/api/fabricantes
/api/categorias
/api/clientes
/api/veiculos
/api/alugueis
/api/pagamentos
```

Filtros:

```text
/api/filtros/veiculos-por-fabricante/{fabricanteId}
/api/filtros/veiculos-por-categoria/{categoriaId}
/api/filtros/alugueis-por-cliente/{clienteId}
/api/filtros/veiculos-sem-aluguel-ativo
/api/filtros/alugueis-com-pagamento
```

## Observação sobre migrations

Para versionar o banco com migrations, instale a ferramenta:

```bash
dotnet tool install --global dotnet-ef
```

Depois:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

A aplicação também possui `EnsureCreated()` no `Program.cs` para facilitar a primeira execução local.
