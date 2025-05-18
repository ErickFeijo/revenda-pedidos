using RevendaPedidos.Application.DTOs;

namespace RevendaPedidos.Application.Interfaces.Services;

public interface IPedidoService
{
    Task<Guid> RegistrarPedidoAsync(PedidoDTO pedidoDto);
    Task<IEnumerable<PedidoDTO>> ObterPedidosPorRevendaAsync(Guid revendaId);
    Task<PedidoDTO?> ObterPorIdAsync(Guid revendaId, Guid pedidoId);
    Task<IEnumerable<PedidoDTO>> EmitirPedidosParaFornecedorAsync(Guid revendaId, List<Guid> pedidoIds);
}
