using RevendaPedidos.Application.DTOs;

namespace RevendaPedidos.Application.Interfaces.Services;

public interface IPedidoIntegracaoService
{
    Task ProcessarIntegracaoAsync(PedidoFilaDto dto);
}
