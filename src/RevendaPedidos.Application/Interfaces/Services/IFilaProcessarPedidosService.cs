using RevendaPedidos.Application.DTOs;

namespace RevendaPedidos.Application.Interfaces.Services;

public interface IFilaProcessarPedidosService
{
    Task PublicarPedidoAsync(PedidoFilaDto message);
}
