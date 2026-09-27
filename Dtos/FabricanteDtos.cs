namespace LocadoraVeiculos.Dtos;

public record FabricanteDto(int Id, string Nome, string? PaisOrigem);
public record FabricanteInput(string Nome, string? PaisOrigem);
