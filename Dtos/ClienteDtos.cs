namespace LocadoraVeiculos.Dtos;

public record ClienteDto(int Id, string Nome, string CPF, string Email, string? Telefone);
public record ClienteInput(string Nome, string CPF, string Email, string? Telefone);
