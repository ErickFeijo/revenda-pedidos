using MassTransit;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace RevendaPedidos.Infra.Messaging
{
    public class PedidoFilaPublisher : IPedidoFilaPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public PedidoFilaPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublicarPedidoAsync(PedidoFilaDto dto)
        {
            return _publishEndpoint.Publish(dto);
        }
    }
}
