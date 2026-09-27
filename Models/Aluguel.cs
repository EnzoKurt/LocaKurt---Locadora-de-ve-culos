using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Aluguel
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int VeiculoId { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    [Required]
    public DateTime DataFim { get; set; }

    public DateTime? DataDevolucao { get; set; }

    [Range(0, 2000000)]
    public decimal QuilometragemInicial { get; set; }

    [Range(0, 2000000)]
    public decimal? QuilometragemFinal { get; set; }

    [Range(0.01, 100000)]
    public decimal ValorDiaria { get; set; }

    [Range(0, 10000000)]
    public decimal ValorTotal { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } = "Reservado";

    public Cliente? Cliente { get; set; }
    public Veiculo? Veiculo { get; set; }
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
