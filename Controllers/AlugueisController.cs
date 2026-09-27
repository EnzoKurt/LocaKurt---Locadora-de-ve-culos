using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlugueisController : ControllerBase
{
    private readonly LocadoraContext _context;

    public AlugueisController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AluguelDto>>> GetAll()
    {
        var alugueis = await _context.Alugueis
            .AsNoTracking()
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .Select(a => ToDto(a))
            .ToListAsync();

        return Ok(alugueis);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AluguelDto>> GetById(int id)
    {
        var aluguel = await _context.Alugueis
            .AsNoTracking()
            .Include(a => a.Cliente)
            .Include(a => a.Veiculo)
            .Where(a => a.Id == id)
            .Select(a => ToDto(a))
            .FirstOrDefaultAsync();

        if (aluguel is null)
            throw new KeyNotFoundException("Aluguel não encontrado.");

        return Ok(aluguel);
    }

    [HttpPost]
    public async Task<ActionResult<AluguelDto>> Create(AluguelInput input)
    {
        if (input.DataFim <= input.DataInicio)
            throw new InvalidOperationException(
                "A data final deve ser posterior à data inicial.");

        var cliente = await _context.Clientes.FindAsync(input.ClienteId);

        if (cliente is null)
            throw new InvalidOperationException("O cliente informado não existe.");

        var veiculo = await _context.Veiculos.FindAsync(input.VeiculoId);

        if (veiculo is null)
            throw new InvalidOperationException("O veículo informado não existe.");

        if (!veiculo.Disponivel)
            throw new InvalidOperationException("O veículo não está disponível para aluguel.");

        var conflito = await _context.Alugueis.AnyAsync(a =>
            a.VeiculoId == input.VeiculoId &&
            a.Status != "Finalizado" &&
            input.DataInicio < a.DataFim &&
            input.DataFim > a.DataInicio);

        if (conflito)
            throw new InvalidOperationException(
                "Já existe um aluguel ou reserva para esse veículo no período informado.");

        var dias = Math.Max(1, (input.DataFim.Date - input.DataInicio.Date).Days);

        var aluguel = new Aluguel
        {
            ClienteId = input.ClienteId,
            VeiculoId = input.VeiculoId,
            DataInicio = input.DataInicio,
            DataFim = input.DataFim,
            QuilometragemInicial = veiculo.Quilometragem,
            ValorDiaria = veiculo.ValorDiaria,
            ValorTotal = dias * veiculo.ValorDiaria,
            Status = "Reservado"
        };

        // O veículo fica indisponível assim que existe uma reserva/aluguel ativo.
        veiculo.Disponivel = false;

        _context.Alugueis.Add(aluguel);
        await _context.SaveChangesAsync();

        await _context.Entry(aluguel).Reference(a => a.Cliente).LoadAsync();
        await _context.Entry(aluguel).Reference(a => a.Veiculo).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = aluguel.Id }, ToDto(aluguel));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, AluguelInput input)
    {
        if (input.DataFim <= input.DataInicio)
            throw new InvalidOperationException(
                "A data final deve ser posterior à data inicial.");

        var aluguel = await _context.Alugueis
            .Include(a => a.Veiculo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel is null)
            throw new KeyNotFoundException("Aluguel não encontrado.");

        if (aluguel.DataDevolucao.HasValue || aluguel.Status == "Finalizado")
            throw new InvalidOperationException(
                "Um aluguel já finalizado não pode ser alterado.");

        if (!await _context.Clientes.AnyAsync(c => c.Id == input.ClienteId))
            throw new InvalidOperationException("O cliente informado não existe.");

        if (!await _context.Veiculos.AnyAsync(v => v.Id == input.VeiculoId))
            throw new InvalidOperationException("O veículo informado não existe.");

        var conflito = await _context.Alugueis.AnyAsync(a =>
            a.Id != id &&
            a.VeiculoId == input.VeiculoId &&
            a.Status != "Finalizado" &&
            input.DataInicio < a.DataFim &&
            input.DataFim > a.DataInicio);

        if (conflito)
            throw new InvalidOperationException(
                "Já existe outro aluguel ou reserva para esse veículo no período informado.");

        var veiculo = await _context.Veiculos.FindAsync(input.VeiculoId);

        aluguel.ClienteId = input.ClienteId;
        aluguel.VeiculoId = input.VeiculoId;
        aluguel.DataInicio = input.DataInicio;
        aluguel.DataFim = input.DataFim;
        aluguel.ValorDiaria = veiculo!.ValorDiaria;

        var dias = Math.Max(1, (input.DataFim.Date - input.DataInicio.Date).Days);
        aluguel.ValorTotal = dias * aluguel.ValorDiaria;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id:int}/devolucao")]
    public async Task<IActionResult> Devolver(int id, DevolucaoInput input)
    {
        var aluguel = await _context.Alugueis
            .Include(a => a.Veiculo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel is null)
            throw new KeyNotFoundException("Aluguel não encontrado.");

        if (aluguel.DataDevolucao.HasValue)
            throw new InvalidOperationException("Esse aluguel já foi devolvido.");

        if (input.QuilometragemFinal < aluguel.QuilometragemInicial)
            throw new InvalidOperationException(
                "A quilometragem final não pode ser menor que a inicial.");

        aluguel.DataDevolucao = input.DataDevolucao;
        aluguel.QuilometragemFinal = input.QuilometragemFinal;
        aluguel.Status = "Finalizado";

        aluguel.Veiculo!.Quilometragem = input.QuilometragemFinal;
        aluguel.Veiculo.Disponivel = true;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var aluguel = await _context.Alugueis
            .Include(a => a.Veiculo)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (aluguel is null)
            throw new KeyNotFoundException("Aluguel não encontrado.");

        if (aluguel.DataDevolucao.HasValue)
            throw new InvalidOperationException(
                "Não é possível excluir um aluguel que já foi finalizado.");

        aluguel.Veiculo!.Disponivel = true;

        _context.Alugueis.Remove(aluguel);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static AluguelDto ToDto(Aluguel a)
    {
        return new AluguelDto(
            a.Id,
            a.ClienteId,
            a.Cliente?.Nome,
            a.VeiculoId,
            a.Veiculo?.Modelo,
            a.DataInicio,
            a.DataFim,
            a.DataDevolucao,
            a.QuilometragemInicial,
            a.QuilometragemFinal,
            a.ValorDiaria,
            a.ValorTotal,
            a.Status);
    }
}
