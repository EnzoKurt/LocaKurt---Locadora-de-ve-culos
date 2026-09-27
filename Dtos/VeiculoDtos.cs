namespace LocadoraVeiculos.Dtos;

public record VeiculoDto(
    int Id,
    string Placa,
    string Modelo,
    int AnoFabricacao,
    decimal Quilometragem,
    decimal ValorDiaria,
    bool Disponivel,
    int FabricanteId,
    string? Fabricante,
    int CategoriaId,
    string? Categoria
);

public record VeiculoInput(
    string Placa,
    string Modelo,
    int AnoFabricacao,
    decimal Quilometragem,
    decimal ValorDiaria,
    bool Disponivel,
    int FabricanteId,
    int CategoriaId
);
