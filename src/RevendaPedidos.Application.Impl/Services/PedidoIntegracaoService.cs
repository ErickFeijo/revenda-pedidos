using Microsoft.Extensions.Logging;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;

public class PedidoIntegracaoService : IPedidoIntegracaoService
{
    private readonly IIntegradorFornecedorService _integradorFornecedorService;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ILogger<PedidoIntegracaoService> _logger;

    public PedidoIntegracaoService(
        IIntegradorFornecedorService integradorFornecedorService,
        IPedidoRepository pedidoRepository,
        ILogger<PedidoIntegracaoService> logger)
    {
        _integradorFornecedorService = integradorFornecedorService;
        _pedidoRepository = pedidoRepository;
        _logger = logger;
    }

    public async Task ProcessarIntegracaoAsync(PedidoFilaDto dto)
    {
        try
        {
            await _integradorFornecedorService.EnviarPedidoAsync(dto);

            // Atualiza status para Finalizado
            if (dto.Id.HasValue)
            {
                var pedido = await _pedidoRepository.ObterPorIdAsync(dto.RevendaId, dto.Id.Value);
                if (pedido != null)
                {
                    pedido.AlterarStatus(RevendaPedidos.Domain.Entities.StatusPedido.Finalizado);
                    await _pedidoRepository.AtualizarAsync(pedido);
                }
            }

            _logger.LogInformation("Pedido {Id} integrado com sucesso.", dto.Id);
        }
        catch (Exception ex)
        {
            //TODO: Verificar erro de connecionstring não definida
            _logger.LogWarning(ex, "Erro ao integrar o pedido {Id}", dto.Id);
            throw;
        }
    }
}
