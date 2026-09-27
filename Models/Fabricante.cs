using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Fabricante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do fabricante é obrigatório.")]
    [StringLength(100, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(60)]
    public string? PaisOrigem { get; set; }

    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
