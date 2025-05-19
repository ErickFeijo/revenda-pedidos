using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Application.Mappers;
using RevendaPedidos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;

namespace RevendaPedidos.Application.Impl.Services
{
    public class FilaProcessarPedidosService : IFilaProcessarPedidosService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public FilaProcessarPedidosService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublicarPedidoAsync(PedidoFilaDto message)
        {
            return _publishEndpoint.Publish(message);
        }
    }
}
