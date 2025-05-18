using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;

public class RevendaPedidosDbContext : DbContext
{
    public RevendaPedidosDbContext(DbContextOptions<RevendaPedidosDbContext> options) : base(options) { }

    public DbSet<Revenda> Revendas => Set<Revenda>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Revenda>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.OwnsMany(r => r.Telefones, t =>
            {
                t.WithOwner().HasForeignKey("RevendaId");
                t.HasKey("RevendaId", "Numero"); // chave composta para identificar o telefone
                t.Property(p => p.Numero).IsRequired();
            });

            // Contatos
            entity.OwnsMany(r => r.Contatos, c =>
            {
                c.WithOwner().HasForeignKey("RevendaId");
                c.HasKey("RevendaId", "Nome"); // Exemplo de chave composta
                c.Property(p => p.Nome).IsRequired();
            });

            // Endereços de entrega
            entity.OwnsMany(r => r.EnderecosEntrega, e =>
            {
                e.WithOwner().HasForeignKey("RevendaId");
                e.HasKey("RevendaId", "Nome"); 
            });
        });

    }
}
