using LocadoraVeiculos.Data;
using LocadoraVeiculos.Dtos;
using LocadoraVeiculos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly LocadoraContext _context;

    public ClientesController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDto>>> GetAll()
    {
        var clientes = await _context.Clientes
            .AsNoTracking()
            .Select(c => new ClienteDto(c.Id, c.Nome, c.CPF, c.Email, c.Telefone))
            .ToListAsync();

        return Ok(clientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> GetById(int id)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ClienteDto(c.Id, c.Nome, c.CPF, c.Email, c.Telefone))
            .FirstOrDefaultAsync();

        if (cliente is null)
            throw new KeyNotFoundException("Cliente não encontrado.");

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Create(ClienteInput input)
    {
        var cpf = SomenteNumeros(input.CPF);
        var email = input.Email.Trim().ToLowerInvariant();

        if (cpf.Length != 11)
            throw new InvalidOperationException("O CPF deve possuir 11 números.");

        if (await _context.Clientes.AnyAsync(c => c.CPF == cpf))
            throw new InvalidOperationException("Já existe um cliente com esse CPF.");

        if (await _context.Clientes.AnyAsync(c => c.Email == email))
            throw new InvalidOperationException("Já existe um cliente com esse e-mail.");

        var cliente = new Cliente
        {
            Nome = input.Nome.Trim(),
            CPF = cpf,
            Email = email,
            Telefone = input.Telefone?.Trim()
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        var dto = new ClienteDto(
            cliente.Id,
            cliente.Nome,
            cliente.CPF,
            cliente.Email,
            cliente.Telefone);

        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClienteInput input)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente is null)
            throw new KeyNotFoundException("Cliente não encontrado.");

        var cpf = SomenteNumeros(input.CPF);
        var email = input.Email.Trim().ToLowerInvariant();

        if (cpf.Length != 11)
            throw new InvalidOperationException("O CPF deve possuir 11 números.");

        if (await _context.Clientes.AnyAsync(c => c.Id != id && c.CPF == cpf))
            throw new InvalidOperationException("Outro cliente já utiliza esse CPF.");

        if (await _context.Clientes.AnyAsync(c => c.Id != id && c.Email == email))
            throw new InvalidOperationException("Outro cliente já utiliza esse e-mail.");

        cliente.Nome = input.Nome.Trim();
        cliente.CPF = cpf;
        cliente.Email = email;
        cliente.Telefone = input.Telefone?.Trim();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Alugueis)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente is null)
            throw new KeyNotFoundException("Cliente não encontrado.");

        if (cliente.Alugueis.Count > 0)
            throw new InvalidOperationException(
                "Não é possível excluir o cliente porque existem aluguéis vinculados a ele.");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static string SomenteNumeros(string valor)
    {
        return new string(valor.Where(char.IsDigit).ToArray());
    }
}
