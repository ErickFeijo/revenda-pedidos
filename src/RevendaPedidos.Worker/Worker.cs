using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class PedidoIntegracaoWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PedidoIntegracaoWorker> _logger;

    public PedidoIntegracaoWorker(IServiceProvider serviceProvider, ILogger<PedidoIntegracaoWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    //var pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();
                    //var integradorService = scope.ServiceProvider.GetRequiredService<IIntegradorAmbevService>();

                    //// Buscar pedidos "AguardandoIntegracao" (exemplo, ajuste pro seu modelo)
                    //var pedidos = await pedidoRepository.ListarPorStatusAsync(StatusPedido.AguardandoIntegracao);

                    //foreach (var pedido in pedidos)
                    //{
                    //    try
                    //    {
                    //        // Envia para "AMBEV"
                    //        await integradorService.EnviarPedidoAsync(pedido);

                    //        // Atualiza o status
                    //        pedido.AlterarStatus(StatusPedido.Finalizado);
                    //        await pedidoRepository.AtualizarAsync(pedido);

                            _logger.LogInformation("Pedido {PedidoId} integrado com sucesso");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        _logger.LogError(ex, "Falha ao integrar pedido {PedidoId}", pedido.Id);

                    //        // Marca como falha de integração
                    //        pedido.AlterarStatus(StatusPedido.FalhaIntegracao);
                    //        await pedidoRepository.AtualizarAsync(pedido);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral no processamento do worker");
            }

            // Aguarda X segundos entre ciclos
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
