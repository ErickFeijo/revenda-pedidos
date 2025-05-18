namespace RevendaPedidos.Application.DTOs;

public class PedidoDTO
{
    public Guid? Id { get; set; }
    public Guid RevendaId { get; set; }
    public ClienteFinalDTO ClienteFinal { get; set; }
    public List<ItemPedidoDTO> Itens { get; set; } = new();
    public DateTime DataCriacao { get; set; }
    public string? Status { get; set; }
    public decimal Total { get; set; }
}

public class ClienteFinalDTO
{
    public string Nome { get; set; }
    public string? Documento { get; set; }
}

public class ItemPedidoDTO
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; }
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    public decimal Total { get; set; }
}
