namespace LocadoraVeiculos.Dtos;

public record AluguelDto(
    int Id,
    int ClienteId,
    string? Cliente,
    int VeiculoId,
    string? Veiculo,
    DateTime DataInicio,
    DateTime DataFim,
    DateTime? DataDevolucao,
    decimal QuilometragemInicial,
    decimal? QuilometragemFinal,
    decimal ValorDiaria,
    decimal ValorTotal,
    string Status
);

public record AluguelInput(
    int ClienteId,
    int VeiculoId,
    DateTime DataInicio,
    DateTime DataFim
);

public record DevolucaoInput(
    DateTime DataDevolucao,
    decimal QuilometragemFinal
);
