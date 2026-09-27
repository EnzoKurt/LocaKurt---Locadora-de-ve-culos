using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagamentosController : ControllerBase
{
    private readonly LocadoraContext _context;

    public PagamentosController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PagamentoDto>>> GetAll()
    {
        var pagamentos = await _context.Pagamentos
            .AsNoTracking()
            .Select(p => new PagamentoDto(
                p.Id,
                p.AluguelId,
                p.DataPagamento,
                p.Valor,
                p.FormaPagamento,
                p.Status))
            .ToListAsync();

        return Ok(pagamentos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PagamentoDto>> GetById(int id)
    {
        var pagamento = await _context.Pagamentos
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PagamentoDto(
                p.Id,
                p.AluguelId,
                p.DataPagamento,
                p.Valor,
                p.FormaPagamento,
                p.Status))
            .FirstOrDefaultAsync();

        if (pagamento is null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        return Ok(pagamento);
    }

    [HttpPost]
    public async Task<ActionResult<PagamentoDto>> Create(PagamentoInput input)
    {
        if (!await _context.Alugueis.AnyAsync(a => a.Id == input.AluguelId))
            throw new InvalidOperationException("O aluguel informado não existe.");

        if (input.Valor <= 0)
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        var pagamento = new Pagamento
        {
            AluguelId = input.AluguelId,
            DataPagamento = input.DataPagamento,
            Valor = input.Valor,
            FormaPagamento = input.FormaPagamento.Trim(),
            Status = input.Status.Trim()
        };

        _context.Pagamentos.Add(pagamento);
        await _context.SaveChangesAsync();

        var dto = new PagamentoDto(
            pagamento.Id,
            pagamento.AluguelId,
            pagamento.DataPagamento,
            pagamento.Valor,
            pagamento.FormaPagamento,
            pagamento.Status);

        return CreatedAtAction(nameof(GetById), new { id = pagamento.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PagamentoInput input)
    {
        var pagamento = await _context.Pagamentos.FindAsync(id);

        if (pagamento is null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        if (!await _context.Alugueis.AnyAsync(a => a.Id == input.AluguelId))
            throw new InvalidOperationException("O aluguel informado não existe.");

        if (input.Valor <= 0)
            throw new InvalidOperationException("O valor do pagamento deve ser maior que zero.");

        pagamento.AluguelId = input.AluguelId;
        pagamento.DataPagamento = input.DataPagamento;
        pagamento.Valor = input.Valor;
        pagamento.FormaPagamento = input.FormaPagamento.Trim();
        pagamento.Status = input.Status.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pagamento = await _context.Pagamentos.FindAsync(id);

        if (pagamento is null)
            throw new KeyNotFoundException("Pagamento não encontrado.");

        _context.Pagamentos.Remove(pagamento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
