using MassTransit;
using Microsoft.Extensions.Logging;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using System.Threading.Tasks;

public class PedidoFilaConsumer : IConsumer<PedidoFilaDto>
{
    private readonly ILogger<PedidoFilaConsumer> _logger;
    private readonly IPedidoIntegracaoService _pedidoIntegracaoService;

    public PedidoFilaConsumer(
        ILogger<PedidoFilaConsumer> logger,
        IPedidoIntegracaoService pedidoIntegracaoService)
    {
        _logger = logger;
        _pedidoIntegracaoService = pedidoIntegracaoService;
    }

    public async Task Consume(ConsumeContext<PedidoFilaDto> context)
    {
        _logger.LogInformation("Consumo de PedidoFilaDto iniciado. PedidoId={Id}", context.Message.Id);

        await _pedidoIntegracaoService.ProcessarIntegracaoAsync(context.Message);

        _logger.LogInformation("Consumo de PedidoFilaDto finalizado. PedidoId={Id}", context.Message.Id);
    }
}

