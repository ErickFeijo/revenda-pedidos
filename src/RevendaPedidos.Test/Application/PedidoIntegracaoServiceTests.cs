using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;
using Xunit;

namespace RevendaPedidos.Tests.Application
{
    public class PedidoIntegracaoServiceTests
    {
        private readonly Mock<IIntegradorFornecedorService> _integradorFornecedorService;
        private readonly Mock<IPedidoRepository> _pedidoRepository;
        private readonly Mock<ILogger<PedidoIntegracaoService>> _logger;
        private readonly PedidoIntegracaoService _service;

        public PedidoIntegracaoServiceTests()
        {
            _integradorFornecedorService = new Mock<IIntegradorFornecedorService>();
            _pedidoRepository = new Mock<IPedidoRepository>();
            _logger = new Mock<ILogger<PedidoIntegracaoService>>();
            _service = new PedidoIntegracaoService(_integradorFornecedorService.Object, _pedidoRepository.Object, _logger.Object);
        }

        [Fact]
        public async Task ProcessarIntegracaoAsync_Deve_IntegrarEPersistirPedido_Finalizado()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var revendaId = Guid.NewGuid();
            var dto = new PedidoFilaDto
            {
                Id = pedidoId,
                RevendaId = revendaId,
                Itens = new List<ItemPedidoFilaDTO>
                {
                    new ItemPedidoFilaDTO { ProdutoId = Guid.NewGuid(), Quantidade = 5 }
                },
                DataCriacao = DateTime.Now
            };

            var cliente = new ClienteFinal("Cliente", "123456");
            var itens = new List<ItemPedido>
            {
                new ItemPedido(Guid.NewGuid(), "Produto Teste", 10.0m, 5)
            };

            var pedido = new Pedido(revendaId, cliente, itens);
            var statusAntes = pedido.Status;

            _integradorFornecedorService.Setup(s => s.EnviarPedidoAsync(dto)).Returns(Task.CompletedTask);
            _pedidoRepository.Setup(r => r.ObterPorIdAsync(dto.RevendaId, dto.Id.Value))
                .ReturnsAsync(pedido);
            _pedidoRepository.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>())).Returns(Task.CompletedTask);

            // Act
            await _service.ProcessarIntegracaoAsync(dto);

            // Assert
            _integradorFornecedorService.Verify(s => s.EnviarPedidoAsync(dto), Times.Once);
            _pedidoRepository.Verify(r => r.ObterPorIdAsync(dto.RevendaId, dto.Id.Value), Times.Once);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.Is<Pedido>(p => p.Status == StatusPedido.Finalizado)), Times.Once);
            Assert.Equal(StatusPedido.Finalizado, pedido.Status);
        }

        [Fact]
        public async Task ProcessarIntegracaoAsync_DeveChamarSoIntegracao_QuandoIdForNull()
        {
            // Arrange
            var dto = new PedidoFilaDto
            {
                Id = null,
                RevendaId = Guid.NewGuid(),
                Itens = new List<ItemPedidoFilaDTO>
                {
                    new ItemPedidoFilaDTO { ProdutoId = Guid.NewGuid(), Quantidade = 3 }
                },
                DataCriacao = DateTime.Now
            };

            _integradorFornecedorService.Setup(s => s.EnviarPedidoAsync(dto)).Returns(Task.CompletedTask);

            // Act
            await _service.ProcessarIntegracaoAsync(dto);

            // Assert
            _integradorFornecedorService.Verify(s => s.EnviarPedidoAsync(dto), Times.Once);
            _pedidoRepository.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact]
        public async Task ProcessarIntegracaoAsync_DeveLancarExcecao_QuandoIntegracaoFalha()
        {
            // Arrange
            var dto = new PedidoFilaDto
            {
                Id = Guid.NewGuid(),
                RevendaId = Guid.NewGuid(),
                Itens = new List<ItemPedidoFilaDTO>
                {
                    new ItemPedidoFilaDTO { ProdutoId = Guid.NewGuid(), Quantidade = 2 }
                },
                DataCriacao = DateTime.Now
            };
            _integradorFornecedorService.Setup(s => s.EnviarPedidoAsync(dto))
                .ThrowsAsync(new InvalidOperationException("Falha ao enviar"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ProcessarIntegracaoAsync(dto));
            _pedidoRepository.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Never);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact]
        public async Task ProcessarIntegracaoAsync_NaoAlteraStatus_SePedidoNaoEncontrado()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var revendaId = Guid.NewGuid();
            var dto = new PedidoFilaDto
            {
                Id = pedidoId,
                RevendaId = revendaId,
                Itens = new List<ItemPedidoFilaDTO>
                {
                    new ItemPedidoFilaDTO { ProdutoId = Guid.NewGuid(), Quantidade = 1 }
                },
                DataCriacao = DateTime.Now
            };
            _integradorFornecedorService.Setup(s => s.EnviarPedidoAsync(dto)).Returns(Task.CompletedTask);
            _pedidoRepository.Setup(r => r.ObterPorIdAsync(revendaId, pedidoId))
                .ReturnsAsync((Pedido)null!);

            // Act
            await _service.ProcessarIntegracaoAsync(dto);

            // Assert
            _integradorFornecedorService.Verify(s => s.EnviarPedidoAsync(dto), Times.Once);
            _pedidoRepository.Verify(r => r.ObterPorIdAsync(revendaId, pedidoId), Times.Once);
            _pedidoRepository.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
        }
    }
}
