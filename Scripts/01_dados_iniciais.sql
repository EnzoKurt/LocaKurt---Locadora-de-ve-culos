USE LocadoraVeiculos;
GO

-- Dados iniciais simples para facilitar os primeiros testes da API.

INSERT INTO Fabricantes (Nome, PaisOrigem)
VALUES
('Toyota', 'Japão'),
('Chevrolet', 'Estados Unidos'),
('Volkswagen', 'Alemanha');

INSERT INTO Categorias (Nome, Descricao)
VALUES
('Econômico', 'Veículos compactos para uso urbano'),
('SUV', 'Veículos utilitários esportivos'),
('Sedan', 'Veículos para uso urbano e rodoviário');

INSERT INTO Clientes (Nome, CPF, Email, Telefone)
VALUES
('João da Silva', '12345678901', 'joao@email.com', '31999990001'),
('Maria Oliveira', '98765432100', 'maria@email.com', '31999990002');

INSERT INTO Veiculos
(Placa, Modelo, AnoFabricacao, Quilometragem, ValorDiaria, Disponivel, FabricanteId, CategoriaId)
VALUES
('ABC1D23', 'Corolla', 2023, 35000, 180.00, 1, 1, 3),
('DEF4G56', 'Onix', 2024, 18000, 120.00, 1, 2, 1),
('GHI7J89', 'T-Cross', 2023, 27000, 160.00, 1, 3, 2);
