using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FabricantesController : ControllerBase
{
    private readonly LocadoraContext _context;

    public FabricantesController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FabricanteDto>>> GetAll()
    {
        var fabricantes = await _context.Fabricantes
            .AsNoTracking()
            .Select(f => new FabricanteDto(f.Id, f.Nome, f.PaisOrigem))
            .ToListAsync();

        return Ok(fabricantes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FabricanteDto>> GetById(int id)
    {
        var fabricante = await _context.Fabricantes
            .AsNoTracking()
            .Where(f => f.Id == id)
            .Select(f => new FabricanteDto(f.Id, f.Nome, f.PaisOrigem))
            .FirstOrDefaultAsync();

        if (fabricante is null)
            throw new KeyNotFoundException("Fabricante não encontrado.");

        return Ok(fabricante);
    }

    [HttpPost]
    public async Task<ActionResult<FabricanteDto>> Create(FabricanteInput input)
    {
        var nome = input.Nome.Trim();

        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome do fabricante é obrigatório.");

        if (await _context.Fabricantes.AnyAsync(f => f.Nome == nome))
            throw new InvalidOperationException("Já existe um fabricante com esse nome.");

        var fabricante = new Fabricante
        {
            Nome = nome,
            PaisOrigem = input.PaisOrigem?.Trim()
        };

        _context.Fabricantes.Add(fabricante);
        await _context.SaveChangesAsync();

        var dto = new FabricanteDto(fabricante.Id, fabricante.Nome, fabricante.PaisOrigem);

        return CreatedAtAction(nameof(GetById), new { id = fabricante.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, FabricanteInput input)
    {
        var fabricante = await _context.Fabricantes.FindAsync(id);

        if (fabricante is null)
            throw new KeyNotFoundException("Fabricante não encontrado.");

        var nome = input.Nome.Trim();

        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome do fabricante é obrigatório.");

        if (await _context.Fabricantes.AnyAsync(f => f.Id != id && f.Nome == nome))
            throw new InvalidOperationException("Já existe outro fabricante com esse nome.");

        fabricante.Nome = nome;
        fabricante.PaisOrigem = input.PaisOrigem?.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var fabricante = await _context.Fabricantes
            .Include(f => f.Veiculos)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (fabricante is null)
            throw new KeyNotFoundException("Fabricante não encontrado.");

        if (fabricante.Veiculos.Count > 0)
            throw new InvalidOperationException(
                "Não é possível excluir o fabricante porque existem veículos vinculados a ele.");

        _context.Fabricantes.Remove(fabricante);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
