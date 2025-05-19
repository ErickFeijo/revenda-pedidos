using RevendaPedidos.Application.DTOs;
using System.Threading.Tasks;

namespace RevendaPedidos.Application.Interfaces.Services
{
    public interface IPedidoFilaPublisher
    {
        Task PublicarPedidoAsync(PedidoFilaDto dto);
    }
}
