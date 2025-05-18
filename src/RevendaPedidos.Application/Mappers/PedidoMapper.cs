using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Domain.Entities;

namespace RevendaPedidos.Application.Mappers
{
    public static class PedidoMapper
    {
        public static Pedido Map(this PedidoDTO dto)
        {
            var status = Enum.TryParse<StatusPedido>(dto.Status, true, out var statusPedido)
                ? statusPedido
                : StatusPedido.Pendente;

            var itens = dto.Itens?.Select(i => new ItemPedido(
                i.ProdutoId,
                i.ProdutoNome,
                i.PrecoUnitario,
                i.Quantidade
            )).ToList() ?? new List<ItemPedido>();

            var clienteFinal = !string.IsNullOrWhiteSpace(dto.ClienteFinal.Nome)
                ? new ClienteFinal(dto.ClienteFinal.Nome, dto.ClienteFinal.Documento)
                : throw new ArgumentException("Nome do cliente final é obrigatório.");

            var pedido = new Pedido(
                dto.RevendaId,
                clienteFinal,
                itens
            );

            if (status != StatusPedido.Pendente)
                pedido.AlterarStatus(status);

            return pedido;
        }

        public static PedidoDTO Map(this Pedido entity)
        {
            return new PedidoDTO
            {
                Id = entity.Id,
                RevendaId = entity.RevendaId,
                ClienteFinal = new ClienteFinalDTO
                    {
                        Nome = entity.ClienteFinal.Nome,
                        Documento = entity.ClienteFinal.Documento
                    }, 
                DataCriacao = entity.DataCriacao,
                Status = entity.Status.ToString(),
                Itens = entity.Itens?.Select(i => new ItemPedidoDTO
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    PrecoUnitario = i.PrecoUnitario,
                    Quantidade = i.Quantidade,
                    Total = i.Total
                }).ToList() ?? new List<ItemPedidoDTO>()
            };
        }
    }
}
