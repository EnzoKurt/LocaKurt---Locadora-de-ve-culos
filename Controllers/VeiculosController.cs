using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly LocadoraContext _context;

    public VeiculosController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VeiculoDto>>> GetAll()
    {
        var veiculos = await _context.Veiculos
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .Include(v => v.Categoria)
            .Select(v => ToDto(v))
            .ToListAsync();

        return Ok(veiculos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VeiculoDto>> GetById(int id)
    {
        var veiculo = await _context.Veiculos
            .AsNoTracking()
            .Include(v => v.Fabricante)
            .Include(v => v.Categoria)
            .Where(v => v.Id == id)
            .Select(v => ToDto(v))
            .FirstOrDefaultAsync();

        if (veiculo is null)
            throw new KeyNotFoundException("Veículo não encontrado.");

        return Ok(veiculo);
    }

    [HttpPost]
    public async Task<ActionResult<VeiculoDto>> Create(VeiculoInput input)
    {
        var placa = input.Placa.Trim().ToUpperInvariant();

        if (await _context.Veiculos.AnyAsync(v => v.Placa == placa))
            throw new InvalidOperationException("Já existe um veículo com essa placa.");

        await ValidarDependencias(input.FabricanteId, input.CategoriaId);

        var veiculo = new Veiculo
        {
            Placa = placa,
            Modelo = input.Modelo.Trim(),
            AnoFabricacao = input.AnoFabricacao,
            Quilometragem = input.Quilometragem,
            ValorDiaria = input.ValorDiaria,
            Disponivel = input.Disponivel,
            FabricanteId = input.FabricanteId,
            CategoriaId = input.CategoriaId
        };

        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();

        await _context.Entry(veiculo).Reference(v => v.Fabricante).LoadAsync();
        await _context.Entry(veiculo).Reference(v => v.Categoria).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = veiculo.Id }, ToDto(veiculo));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, VeiculoInput input)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);

        if (veiculo is null)
            throw new KeyNotFoundException("Veículo não encontrado.");

        var placa = input.Placa.Trim().ToUpperInvariant();

        if (await _context.Veiculos.AnyAsync(v => v.Id != id && v.Placa == placa))
            throw new InvalidOperationException("Outro veículo já utiliza essa placa.");

        await ValidarDependencias(input.FabricanteId, input.CategoriaId);

        veiculo.Placa = placa;
        veiculo.Modelo = input.Modelo.Trim();
        veiculo.AnoFabricacao = input.AnoFabricacao;
        veiculo.Quilometragem = input.Quilometragem;
        veiculo.ValorDiaria = input.ValorDiaria;
        veiculo.Disponivel = input.Disponivel;
        veiculo.FabricanteId = input.FabricanteId;
        veiculo.CategoriaId = input.CategoriaId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var veiculo = await _context.Veiculos
            .Include(v => v.Alugueis)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (veiculo is null)
            throw new KeyNotFoundException("Veículo não encontrado.");

        if (veiculo.Alugueis.Count > 0)
            throw new InvalidOperationException(
                "Não é possível excluir o veículo porque existem aluguéis vinculados a ele.");

        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task ValidarDependencias(int fabricanteId, int categoriaId)
    {
        if (!await _context.Fabricantes.AnyAsync(f => f.Id == fabricanteId))
            throw new InvalidOperationException("O fabricante informado não existe.");

        if (!await _context.Categorias.AnyAsync(c => c.Id == categoriaId))
            throw new InvalidOperationException("A categoria informada não existe.");
    }

    private static VeiculoDto ToDto(Veiculo v)
    {
        return new VeiculoDto(
            v.Id,
            v.Placa,
            v.Modelo,
            v.AnoFabricacao,
            v.Quilometragem,
            v.ValorDiaria,
            v.Disponivel,
            v.FabricanteId,
            v.Fabricante?.Nome,
            v.CategoriaId,
            v.Categoria?.Nome);
    }
}
