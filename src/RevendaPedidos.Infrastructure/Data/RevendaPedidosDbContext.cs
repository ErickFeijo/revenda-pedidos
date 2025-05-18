using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;

public class RevendaPedidosDbContext : DbContext
{
    public RevendaPedidosDbContext(DbContextOptions<RevendaPedidosDbContext> options) : base(options) { }

    public DbSet<Revenda> Revendas => Set<Revenda>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Revenda>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.OwnsMany(r => r.Telefones, t =>
            {
                t.WithOwner().HasForeignKey("RevendaId");
                t.HasKey("RevendaId", "Numero");
                t.Property(p => p.Numero).IsRequired();
            });

            entity.OwnsMany(r => r.Contatos, c =>
            {
                c.WithOwner().HasForeignKey("RevendaId");
                c.HasKey("RevendaId", "Nome");
                c.Property(p => p.Nome).IsRequired();
            });

            entity.OwnsMany(r => r.EnderecosEntrega, e =>
            {
                e.WithOwner().HasForeignKey("RevendaId");
                e.HasKey("RevendaId", "Nome");
            });
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.DataCriacao).IsRequired();
            entity.Property(p => p.Status).HasConversion<string>().IsRequired();

            entity.OwnsOne(p => p.ClienteFinal, cf =>
            {
                cf.Property(x => x.Nome).IsRequired();
                cf.Property(x => x.Documento);
            });

            entity.HasMany(p => p.Itens)
                  .WithOne()
                  .HasForeignKey("PedidoId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.ProdutoId).IsRequired();
            entity.Property(x => x.ProdutoNome).IsRequired();
            entity.Property(x => x.PrecoUnitario).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Quantidade).IsRequired();
            entity.Ignore(x => x.Total);
        });
    }
}
