using Moq;
using RevendaPedidos.Application.Impl.Services;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;

namespace RevendaPedidos.Tests.Application
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _repositoryMock;
        private readonly PedidoService _service;

        public PedidoServiceTests()
        {
            _repositoryMock = new Mock<IPedidoRepository>();
            _service = new PedidoService(_repositoryMock.Object);
        }

        [Fact]
        public async Task MarcarPedidosParaIntegracao_DeveAlterarStatusQuandoValido()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = GetPedido(StatusPedido.Novo, 1000);
            pedido.GetType().GetProperty("Id").SetValue(pedido, pedidoId); // Force setting private setter
            _repositoryMock.Setup(r => r.ListarPorIdsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                           .ReturnsAsync(new List<Pedido> { pedido });
            _repositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
                           .Returns(Task.CompletedTask);

            // Act
            var pedidos = await _service.EmitirPedidosParaFornecedorAsync(Guid.NewGuid(), new List<Guid> { pedidoId });

            // Assert
            Assert.Single(pedidos);
            Assert.Equal(StatusPedido.AguardandoIntegracao, pedido.Status);
            _repositoryMock.Verify(r => r.AtualizarAsync(It.Is<Pedido>(p => p.Status == StatusPedido.AguardandoIntegracao)), Times.Once);
        }

        [Fact]
        public async Task MarcarPedidosParaIntegracao_DeveLancarExcecao_QuandoPedidoNaoEncontrado()
        {
            _repositoryMock.Setup(r => r.ListarPorIdsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<Pedido>());

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EmitirPedidosParaFornecedorAsync(Guid.NewGuid(), new List<Guid> { Guid.NewGuid() })
            );
        }

        [Fact]
        public async Task MarcarPedidosParaIntegracao_DeveLancarExcecao_QuandoPedidoNaoPodeEmitir()
        {
            var pedido = GetPedido(StatusPedido.Finalizado, 1000);
            _repositoryMock.Setup(r => r.ListarPorIdsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<Pedido> { pedido });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EmitirPedidosParaFornecedorAsync(Guid.NewGuid(), new List<Guid> { pedido.Id })
            );
        }

        [Fact]
        public async Task MarcarPedidosParaIntegracao_DeveLancarExcecao_QuandoSomaItensMenorQueMil()
        {
            var pedido = GetPedido(StatusPedido.Novo, 500);
            _repositoryMock.Setup(r => r.ListarPorIdsAsync(It.IsAny<Guid>(), It.IsAny<List<Guid>>()))
                .ReturnsAsync(new List<Pedido> { pedido });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.EmitirPedidosParaFornecedorAsync(Guid.NewGuid(), new List<Guid> { pedido.Id })
            );
        }

        // Helper para criar pedido com itens
        private Pedido GetPedido(StatusPedido status, int quantidade)
        {
            var cliente = new ClienteFinal("Cliente X", "123456789");
            var itens = new List<ItemPedido> { new ItemPedido(Guid.NewGuid(), "Bebida Y", 7, quantidade) };
            var pedido = new Pedido(Guid.NewGuid(), cliente, itens);
            pedido.AlterarStatus(status);
            return pedido;
        }
    }
}
