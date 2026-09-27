namespace LocadoraVeiculos.Dtos;

public record CategoriaDto(int Id, string Nome, string? Descricao);
public record CategoriaInput(string Nome, string? Descricao);
