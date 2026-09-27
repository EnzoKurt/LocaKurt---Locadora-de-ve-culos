using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [StringLength(60, MinimumLength = 2)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(250)]
    public string? Descricao { get; set; }

    public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
}
