namespace RevendaPedidos.Api.Models.Requests;

public class PedidoRequest
{
    public string ClienteNome { get; set; }
    public string? ClienteDocumento { get; set; }
    public List<ItemPedidoRequest> Itens { get; set; } = new();
}

public class ItemPedidoRequest
{
    public Guid ProdutoId { get; set; }
    public string ProdutoNome { get; set; }
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
}
