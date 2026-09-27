# ETAPA 2 - IMPLEMENTAÇÃO DO BACKEND

## 2.1 ASP.NET Core

O backend foi desenvolvido em C# utilizando ASP.NET Core Web API.

A aplicação possui Controllers separados por entidade e um controller específico para os filtros.

## 2.2 CRUD

Foram implementadas operações CRUD para:

- Fabricantes
- Categorias
- Clientes
- Veículos
- Aluguéis
- Pagamentos

Operações principais:

```text
GET     consulta registros
GET /id consulta um registro específico
POST    cria um registro
PUT     atualiza um registro
DELETE  exclui um registro
```

Além do CRUD, o aluguel possui uma rota específica para registrar a devolução:

```text
POST /api/alugueis/{id}/devolucao
```

## 2.3 Entity Framework + SQL Express

A conexão é configurada no `appsettings.json`.

O `LocadoraContext` utiliza:

```text
Microsoft.EntityFrameworkCore.SqlServer
```

A string de conexão padrão está configurada para uma instância local:

```text
.\SQLEXPRESS
```

## 2.4 Validação e tratamento de erros

Foram utilizadas:

- Data Annotations nas entidades;
- validações manuais nos controllers;
- validação de chaves estrangeiras;
- verificação de CPF, e-mail e placa duplicados;
- validação de datas;
- validação da disponibilidade do veículo;
- verificação de conflito de período;
- validação da quilometragem na devolução;
- middleware global para tratamento de exceções.

Erros comuns retornam HTTP 400 ou 404 com uma mensagem em JSON.

Erros internos não expõem detalhes da exceção para o cliente.

## 2.5 Cinco filtros com JOIN

Foram criadas cinco rotas:

### 1. Veículos por fabricante

```text
GET /api/filtros/veiculos-por-fabricante/{fabricanteId}
```

Tipo de JOIN: INNER JOIN.

Tabelas envolvidas: Fabricante e Veiculo.

### 2. Veículos por categoria

```text
GET /api/filtros/veiculos-por-categoria/{categoriaId}
```

Tipo de JOIN: INNER JOIN.

Tabelas envolvidas: Categoria e Veiculo.

### 3. Aluguéis por cliente

```text
GET /api/filtros/alugueis-por-cliente/{clienteId}
```

Tipos de JOIN utilizados: INNER JOIN.

Tabelas envolvidas: Cliente, Aluguel e Veiculo.

### 4. Veículos sem aluguel ativo

```text
GET /api/filtros/veiculos-sem-aluguel-ativo
```

Tipo de JOIN: LEFT JOIN.

Tabelas envolvidas: Veiculo e Aluguel.

### 5. Aluguéis com pagamento

```text
GET /api/filtros/alugueis-com-pagamento
```

Tipo de JOIN: LEFT JOIN.

Tabelas envolvidas: Aluguel e Pagamento.

Portanto, o projeto utiliza pelo menos dois tipos de JOIN:

- INNER JOIN
- LEFT JOIN
