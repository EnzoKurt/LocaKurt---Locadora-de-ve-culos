using LocadoraVeiculos.Models;
using Microsoft.EntityFrameworkCore;

namespace LocadoraVeiculos.Data;

public class LocadoraContext : DbContext
{
    public LocadoraContext(DbContextOptions<LocadoraContext> options)
        : base(options)
    {
    }

    public DbSet<Fabricante> Fabricantes => Set<Fabricante>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Aluguel> Alugueis => Set<Aluguel>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Fabricante>()
            .HasIndex(f => f.Nome)
            .IsUnique();

        modelBuilder.Entity<Categoria>()
            .HasIndex(c => c.Nome)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CPF)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Veiculo>()
            .HasIndex(v => v.Placa)
            .IsUnique();

        modelBuilder.Entity<Veiculo>()
            .Property(v => v.Quilometragem)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Veiculo>()
            .Property(v => v.ValorDiaria)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Aluguel>()
            .Property(a => a.QuilometragemInicial)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Aluguel>()
            .Property(a => a.QuilometragemFinal)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Aluguel>()
            .Property(a => a.ValorDiaria)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Aluguel>()
            .Property(a => a.ValorTotal)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.Valor)
            .HasPrecision(12, 2);

        modelBuilder.Entity<Fabricante>()
            .HasMany(f => f.Veiculos)
            .WithOne(v => v.Fabricante)
            .HasForeignKey(v => v.FabricanteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Categoria>()
            .HasMany(c => c.Veiculos)
            .WithOne(v => v.Categoria)
            .HasForeignKey(v => v.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Alugueis)
            .WithOne(a => a.Cliente)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Veiculo>()
            .HasMany(v => v.Alugueis)
            .WithOne(a => a.Veiculo)
            .HasForeignKey(a => a.VeiculoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Aluguel>()
            .HasMany(a => a.Pagamentos)
            .WithOne(p => p.Aluguel)
            .HasForeignKey(p => p.AluguelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
