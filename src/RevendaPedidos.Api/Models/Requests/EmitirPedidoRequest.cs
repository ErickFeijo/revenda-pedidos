namespace RevendaPedidos.Api.Models.Requests;

public class EmitirPedidoRequest
{
    public List<Guid> PedidoIds { get; set; }
}
