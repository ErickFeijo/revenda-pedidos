using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Api.Models.Requests;

namespace RevendaPedidos.Api.Mappers;

public static class PedidoMapper
{
    public static PedidoDTO Map(this PedidoRequest req)
    {
        return new PedidoDTO
        {
            ClienteFinal = new ClienteFinalDTO
            {
                Nome = req.ClienteNome,
                Documento = req.ClienteDocumento
            },
            Itens = req.Itens.Select(i => new ItemPedidoDTO
            {
                ProdutoId = i.ProdutoId,
                ProdutoNome = i.ProdutoNome,
                PrecoUnitario = i.PrecoUnitario,
                Quantidade = i.Quantidade,
                Total = i.PrecoUnitario * i.Quantidade
            }).ToList()
        };
    }
}
