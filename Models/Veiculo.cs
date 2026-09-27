using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Veiculo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A placa é obrigatória.")]
    [StringLength(7, MinimumLength = 7)]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "O modelo é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public string Modelo { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Informe um ano de fabricação válido.")]
    public int AnoFabricacao { get; set; }

    [Range(0, 2000000, ErrorMessage = "A quilometragem deve ser maior ou igual a zero.")]
    public decimal Quilometragem { get; set; }

    [Range(0.01, 100000, ErrorMessage = "O valor da diária deve ser maior que zero.")]
    public decimal ValorDiaria { get; set; }

    public bool Disponivel { get; set; } = true;

    [Required]
    public int FabricanteId { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    public Fabricante? Fabricante { get; set; }
    public Categoria? Categoria { get; set; }
    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
}
