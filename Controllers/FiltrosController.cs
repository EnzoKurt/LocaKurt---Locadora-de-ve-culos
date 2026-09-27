using LocadoraVeiculos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/filtros")]
public class FiltrosController : ControllerBase
{
    private readonly LocadoraContext _context;

    public FiltrosController(LocadoraContext context)
    {
        _context = context;
    }

    // Filtro 1 - INNER JOIN entre veículo e fabricante.
    [HttpGet("veiculos-por-fabricante/{fabricanteId:int}")]
    public async Task<IActionResult> VeiculosPorFabricante(int fabricanteId)
    {
        var existe = await _context.Fabricantes.AnyAsync(f => f.Id == fabricanteId);

        if (!existe)
            throw new KeyNotFoundException("Fabricante não encontrado.");

        var resultado = await (
            from v in _context.Veiculos.AsNoTracking()
            join f in _context.Fabricantes.AsNoTracking()
                on v.FabricanteId equals f.Id
            where f.Id == fabricanteId
            select new
            {
                v.Id,
                v.Placa,
                v.Modelo,
                Fabricante = f.Nome,
                v.AnoFabricacao,
                v.Quilometragem,
                v.ValorDiaria,
                v.Disponivel
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    // Filtro 2 - INNER JOIN entre veículo e categoria.
    [HttpGet("veiculos-por-categoria/{categoriaId:int}")]
    public async Task<IActionResult> VeiculosPorCategoria(int categoriaId)
    {
        var existe = await _context.Categorias.AnyAsync(c => c.Id == categoriaId);

        if (!existe)
            throw new KeyNotFoundException("Categoria não encontrada.");

        var resultado = await (
            from v in _context.Veiculos.AsNoTracking()
            join c in _context.Categorias.AsNoTracking()
                on v.CategoriaId equals c.Id
            where c.Id == categoriaId
            select new
            {
                v.Id,
                v.Placa,
                v.Modelo,
                Categoria = c.Nome,
                v.ValorDiaria,
                v.Disponivel
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    // Filtro 3 - INNER JOIN entre aluguel, cliente e veículo.
    [HttpGet("alugueis-por-cliente/{clienteId:int}")]
    public async Task<IActionResult> AlugueisPorCliente(int clienteId)
    {
        var existe = await _context.Clientes.AnyAsync(c => c.Id == clienteId);

        if (!existe)
            throw new KeyNotFoundException("Cliente não encontrado.");

        var resultado = await (
            from a in _context.Alugueis.AsNoTracking()
            join c in _context.Clientes.AsNoTracking()
                on a.ClienteId equals c.Id
            join v in _context.Veiculos.AsNoTracking()
                on a.VeiculoId equals v.Id
            where c.Id == clienteId
            select new
            {
                AluguelId = a.Id,
                Cliente = c.Nome,
                Veiculo = v.Modelo,
                v.Placa,
                a.DataInicio,
                a.DataFim,
                a.DataDevolucao,
                a.ValorTotal,
                a.Status
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    // Filtro 4 - LEFT JOIN entre veículo e aluguel.
    // A ideia é listar veículos que não possuem aluguel ativo no período.
    [HttpGet("veiculos-sem-aluguel-ativo")]
    public async Task<IActionResult> VeiculosSemAluguelAtivo()
    {
        var hoje = DateTime.Now;

        var resultado = await (
            from v in _context.Veiculos.AsNoTracking()
            join a in _context.Alugueis.AsNoTracking()
                .Where(a => a.Status != "Finalizado" &&
                            a.DataInicio <= hoje &&
                            a.DataFim >= hoje)
                on v.Id equals a.VeiculoId into alugueis
            from a in alugueis.DefaultIfEmpty()
            where a == null
            select new
            {
                v.Id,
                v.Placa,
                v.Modelo,
                v.Quilometragem,
                v.ValorDiaria,
                v.Disponivel
            }
        ).ToListAsync();

        return Ok(resultado);
    }

    // Filtro 5 - LEFT JOIN entre aluguel e pagamento.
    // Mostra os aluguéis e, quando existir, o pagamento relacionado.
    [HttpGet("alugueis-com-pagamento")]
    public async Task<IActionResult> AlugueisComPagamento()
    {
        var resultado = await (
            from a in _context.Alugueis.AsNoTracking()
            join p in _context.Pagamentos.AsNoTracking()
                on a.Id equals p.AluguelId into pagamentos
            from p in pagamentos.DefaultIfEmpty()
            select new
            {
                AluguelId = a.Id,
                a.ClienteId,
                a.VeiculoId,
                a.DataInicio,
                a.DataFim,
                a.ValorTotal,
                a.Status,
                PagamentoId = p == null ? (int?)null : p.Id,
                ValorPago = p == null ? (decimal?)null : p.Valor,
                FormaPagamento = p == null ? null : p.FormaPagamento,
                StatusPagamento = p == null ? null : p.Status
            }
        ).ToListAsync();

        return Ok(resultado);
    }
}
