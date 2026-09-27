using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Pagamento
{
    public int Id { get; set; }

    [Required]
    public int AluguelId { get; set; }

    [Required]
    public DateTime DataPagamento { get; set; }

    [Range(0.01, 10000000, ErrorMessage = "O valor do pagamento deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required]
    [StringLength(30)]
    public string FormaPagamento { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Pendente";

    public Aluguel? Aluguel { get; set; }
}
