# ETAPA 1 - MODELAGEM DO BANCO DE DADOS

## 1.1 Modelo conceitual

O sistema foi modelado para uma locadora de veículos.

Entidades:

- Fabricante
- Categoria
- Veículo
- Cliente
- Aluguel
- Pagamento

Relacionamentos:

- Um fabricante possui vários veículos.
- Uma categoria possui vários veículos.
- Um cliente pode realizar vários aluguéis.
- Um veículo pode participar de vários aluguéis ao longo do tempo.
- Um aluguel pode possuir vários pagamentos.

Representação textual:

```text
FABRICANTE (1) -------- (N) VEICULO
CATEGORIA  (1) -------- (N) VEICULO
CLIENTE    (1) -------- (N) ALUGUEL
VEICULO    (1) -------- (N) ALUGUEL
ALUGUEL    (1) -------- (N) PAGAMENTO
```

## 1.2 Tradução para o modelo relacional com Entity Framework

As classes em `Models/` representam as tabelas.

O arquivo `Data/LocadoraContext.cs` registra cada entidade como um `DbSet` e configura os relacionamentos, índices e tipos de dados.

A aplicação usa o provedor `Microsoft.EntityFrameworkCore.SqlServer`, que permite ao Entity Framework trabalhar com o SQL Server Express.

## 1.3 Chaves e restrições

Chaves primárias:

- Fabricante.Id
- Categoria.Id
- Veiculo.Id
- Cliente.Id
- Aluguel.Id
- Pagamento.Id

Chaves estrangeiras:

- Veiculo.FabricanteId
- Veiculo.CategoriaId
- Aluguel.ClienteId
- Aluguel.VeiculoId
- Pagamento.AluguelId

Restrições importantes:

- CPF único.
- E-mail único.
- Placa única.
- Nome do fabricante único.
- Nome da categoria único.
- Relacionamentos configurados com `Restrict` onde a exclusão poderia deixar registros dependentes sem referência.
- Pagamento relacionado ao aluguel com exclusão em cascata.
- Campos numéricos possuem limites e precisão definidos.

## 1.4 Classes de entidades

As classes estão na pasta `Models/`:

- `Fabricante.cs`
- `Categoria.cs`
- `Veiculo.cs`
- `Cliente.cs`
- `Aluguel.cs`
- `Pagamento.cs`

## 1.5 Quantidade mínima de entidades

O enunciado exige no mínimo cinco entidades.

O projeto possui seis:

1. Fabricante
2. Categoria
3. Veículo
4. Cliente
5. Aluguel
6. Pagamento

A entidade adicional é `Pagamento`.
