using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;

public class IntegradorFornecedorMockService : IIntegradorFornecedorService
{
    private static readonly Random _random = new();
    private readonly ILogger<IntegradorFornecedorMockService> _logger;

    public IntegradorFornecedorMockService(ILogger<IntegradorFornecedorMockService> logger)
    {
        _logger = logger;
    }

    public async Task EnviarPedidoAsync(PedidoFilaDto pedido)
    {
        // Simula tempo de resposta variável
        await Task.Delay(_random.Next(300, 800));

        // Simula 20% de falha
        if (_random.NextDouble() < 0.2)
        {
            _logger.LogWarning("Falha simulada ao integrar pedido {Id} com fornecedor.", pedido.Id);
            throw new Exception("Falha simulada na integração com o fornecedor.");
        }

        _logger.LogInformation("Pedido {Id} enviado com sucesso ao fornecedor.", pedido.Id);
    }
}
