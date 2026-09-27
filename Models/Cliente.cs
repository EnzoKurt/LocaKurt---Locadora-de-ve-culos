using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    [StringLength(120, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [StringLength(11, MinimumLength = 11)]
    public string CPF { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(20)]
    public string? Telefone { get; set; }

    public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
}
