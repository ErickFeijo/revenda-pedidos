namespace RevendaPedidos.Application.DTOs;

public class PedidoFilaDto
{
    public Guid? Id { get; set; }
    public Guid RevendaId { get; set; }
    public List<ItemPedidoFilaDTO> Itens { get; set; } = new();
    public DateTime DataCriacao { get; set; }
}

public class ItemPedidoFilaDTO
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
