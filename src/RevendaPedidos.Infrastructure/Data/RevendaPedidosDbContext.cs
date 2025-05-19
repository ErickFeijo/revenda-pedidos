using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;

public class RevendaPedidosDbContext : DbContext
{
    public RevendaPedidosDbContext(DbContextOptions<RevendaPedidosDbContext> options)
        : base(options) { }

    public DbSet<Revenda> Revendas => Set<Revenda>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ------- Revenda -------
        var revendaBuilder = modelBuilder.Entity<Revenda>();
        revendaBuilder.HasKey(r => r.Id);

        // Telefones (backing field)
        revendaBuilder.OwnsMany<Telefone>("_telefones", b =>
        {
            b.WithOwner().HasForeignKey("RevendaId");
            b.HasKey("RevendaId", "Numero");
            b.Property(p => p.Numero).IsRequired();
            // Remova b.Property<int>("Id") se Telefone não tiver Id próprio
        });
        revendaBuilder.Navigation("_telefones").UsePropertyAccessMode(PropertyAccessMode.Field);

        // Contatos (backing field)
        revendaBuilder.OwnsMany<Contato>("_contatos", b =>
        {
            b.WithOwner().HasForeignKey("RevendaId");
            b.HasKey("RevendaId", "Nome");
            b.Property(p => p.Nome).IsRequired();
        });
        revendaBuilder.Navigation("_contatos").UsePropertyAccessMode(PropertyAccessMode.Field);

        // EnderecosEntrega (backing field)
        revendaBuilder.OwnsMany<EnderecoEntrega>("_enderecosEntrega", b =>
        {
            b.WithOwner().HasForeignKey("RevendaId");
            b.HasKey("RevendaId", "Nome");
        });
        revendaBuilder.Navigation("_enderecosEntrega").UsePropertyAccessMode(PropertyAccessMode.Field);

        // Importante! Ignore as propriedades públicas (somente leitura)
        revendaBuilder.Ignore(r => r.Telefones);
        revendaBuilder.Ignore(r => r.Contatos);
        revendaBuilder.Ignore(r => r.EnderecosEntrega);

        // ------- Pedido -------
        var pedidoBuilder = modelBuilder.Entity<Pedido>();
        pedidoBuilder.HasKey(p => p.Id);
        pedidoBuilder.Property(p => p.DataCriacao).IsRequired();
        pedidoBuilder.Property(p => p.Status).HasConversion<string>().IsRequired();

        pedidoBuilder.OwnsOne(p => p.ClienteFinal, cf =>
        {
            cf.Property(x => x.Nome).IsRequired();
            cf.Property(x => x.Documento);
        });

        // Itens do pedido usando backing field
        pedidoBuilder.HasMany<ItemPedido>("_itens")
                     .WithOne()
                     .HasForeignKey("PedidoId")
                     .OnDelete(DeleteBehavior.Cascade);
        pedidoBuilder.Navigation("_itens").UsePropertyAccessMode(PropertyAccessMode.Field);

        // Ignore a propriedade pública de itens
        pedidoBuilder.Ignore(p => p.Itens);

        // ------- ItemPedido -------
        var itemPedidoBuilder = modelBuilder.Entity<ItemPedido>();
        itemPedidoBuilder.HasKey(x => x.Id);
        itemPedidoBuilder.Property(x => x.ProdutoId).IsRequired();
        itemPedidoBuilder.Property(x => x.ProdutoNome).IsRequired();
        itemPedidoBuilder.Property(x => x.PrecoUnitario).HasColumnType("decimal(18,2)").IsRequired();
        itemPedidoBuilder.Property(x => x.Quantidade).IsRequired();
        itemPedidoBuilder.Ignore(x => x.Total);

        base.OnModelCreating(modelBuilder);
    }
}