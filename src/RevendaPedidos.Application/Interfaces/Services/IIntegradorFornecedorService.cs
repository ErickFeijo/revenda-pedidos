using RevendaPedidos.Application.DTOs;

namespace RevendaPedidos.Application.Interfaces.Services;

public interface IIntegradorFornecedorService
{
    Task EnviarPedidoAsync(PedidoFilaDto pedido);
}