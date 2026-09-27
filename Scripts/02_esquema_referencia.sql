CREATE DATABASE LocadoraVeiculos;
GO

USE LocadoraVeiculos;
GO

CREATE TABLE Fabricantes (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome NVARCHAR(100) NOT NULL UNIQUE,
    PaisOrigem NVARCHAR(60) NULL
);

CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome NVARCHAR(60) NOT NULL UNIQUE,
    Descricao NVARCHAR(250) NULL
);

CREATE TABLE Clientes (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nome NVARCHAR(120) NOT NULL,
    CPF NVARCHAR(11) NOT NULL UNIQUE,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Telefone NVARCHAR(20) NULL
);

CREATE TABLE Veiculos (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Placa NVARCHAR(7) NOT NULL UNIQUE,
    Modelo NVARCHAR(100) NOT NULL,
    AnoFabricacao INT NOT NULL,
    Quilometragem DECIMAL(12,2) NOT NULL,
    ValorDiaria DECIMAL(10,2) NOT NULL,
    Disponivel BIT NOT NULL,
    FabricanteId INT NOT NULL,
    CategoriaId INT NOT NULL,
    CONSTRAINT FK_Veiculos_Fabricantes FOREIGN KEY (FabricanteId)
        REFERENCES Fabricantes(Id),
    CONSTRAINT FK_Veiculos_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES Categorias(Id)
);

CREATE TABLE Alugueis (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ClienteId INT NOT NULL,
    VeiculoId INT NOT NULL,
    DataInicio DATETIME2 NOT NULL,
    DataFim DATETIME2 NOT NULL,
    DataDevolucao DATETIME2 NULL,
    QuilometragemInicial DECIMAL(12,2) NOT NULL,
    QuilometragemFinal DECIMAL(12,2) NULL,
    ValorDiaria DECIMAL(10,2) NOT NULL,
    ValorTotal DECIMAL(12,2) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_Alugueis_Clientes FOREIGN KEY (ClienteId)
        REFERENCES Clientes(Id),
    CONSTRAINT FK_Alugueis_Veiculos FOREIGN KEY (VeiculoId)
        REFERENCES Veiculos(Id)
);

CREATE TABLE Pagamentos (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    AluguelId INT NOT NULL,
    DataPagamento DATETIME2 NOT NULL,
    Valor DECIMAL(12,2) NOT NULL,
    FormaPagamento NVARCHAR(30) NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_Pagamentos_Alugueis FOREIGN KEY (AluguelId)
        REFERENCES Alugueis(Id)
        ON DELETE CASCADE
);
GO
