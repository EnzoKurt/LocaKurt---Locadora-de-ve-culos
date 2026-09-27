namespace LocadoraVeiculos.Dtos;

public record PagamentoDto(
    int Id,
    int AluguelId,
    DateTime DataPagamento,
    decimal Valor,
    string FormaPagamento,
    string Status
);

public record PagamentoInput(
    int AluguelId,
    DateTime DataPagamento,
    decimal Valor,
    string FormaPagamento,
    string Status
);
