using RevendaPedidos.Domain.Entities;

namespace RevendaPedidos.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);
        Task<IEnumerable<Pedido>> ListarPorRevendaAsync(Guid revendaId);
        Task<Pedido?> ObterPorIdAsync(Guid revendaId, Guid pedidoId);
        Task<List<Pedido>> ListarPorIdsAsync(Guid revendaId, List<Guid> pedidoIds);
        Task AtualizarAsync(Pedido pedido);
    }
}
