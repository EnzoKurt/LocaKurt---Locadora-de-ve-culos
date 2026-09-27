using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly LocadoraContext _context;

    public CategoriasController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .Select(c => new CategoriaDto(c.Id, c.Nome, c.Descricao))
            .ToListAsync();

        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> GetById(int id)
    {
        var categoria = await _context.Categorias
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoriaDto(c.Id, c.Nome, c.Descricao))
            .FirstOrDefaultAsync();

        if (categoria is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create(CategoriaInput input)
    {
        var nome = input.Nome.Trim();

        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome da categoria é obrigatório.");

        if (await _context.Categorias.AnyAsync(c => c.Nome == nome))
            throw new InvalidOperationException("Já existe uma categoria com esse nome.");

        var categoria = new Categoria
        {
            Nome = nome,
            Descricao = input.Descricao?.Trim()
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        var dto = new CategoriaDto(categoria.Id, categoria.Nome, categoria.Descricao);

        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CategoriaInput input)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        var nome = input.Nome.Trim();

        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome da categoria é obrigatório.");

        if (await _context.Categorias.AnyAsync(c => c.Id != id && c.Nome == nome))
            throw new InvalidOperationException("Já existe outra categoria com esse nome.");

        categoria.Nome = nome;
        categoria.Descricao = input.Descricao?.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Veiculos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
            throw new KeyNotFoundException("Categoria não encontrada.");

        if (categoria.Veiculos.Count > 0)
            throw new InvalidOperationException(
                "Não é possível excluir a categoria porque existem veículos vinculados a ela.");

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
