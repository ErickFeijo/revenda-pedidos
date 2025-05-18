namespace RevendaPedidos.Domain.Entities;

public class Pedido
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RevendaId { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public List<ItemPedido> Itens { get; set; } = new();
    public DateTime Data { get; set; } = DateTime.UtcNow;
}
