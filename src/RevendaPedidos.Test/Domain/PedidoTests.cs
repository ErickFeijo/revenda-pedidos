using System;
using System.Collections.Generic;
using System.Linq;
using RevendaPedidos.Domain.Entities;
using Xunit;

namespace RevendaPedidos.Tests.Domain
{
    public class PedidoTests
    {
        [Fact]
        public void Deve_Criar_Pedido_Valido()
        {
            var pedido = CriarPedidoValido();

            Assert.NotEqual(Guid.Empty, pedido.Id);
            Assert.Equal("João", pedido.ClienteFinal.Nome);
            Assert.Single(pedido.Itens);
            Assert.Equal(StatusPedido.Novo, pedido.Status);
        }

        [Fact]
        public void Nao_Deve_Criar_Pedido_Com_RevendaId_Vazio()
        {
            var cliente = new ClienteFinal("João", null);
            var itens = new List<ItemPedido>
            {
                new ItemPedido(Guid.NewGuid(), "Produto 1", 10m, 2)
            };

            Assert.Throws<ArgumentException>(() =>
                new Pedido(Guid.Empty, cliente, itens));
        }

        [Fact]
        public void Nao_Deve_Criar_Pedido_Sem_ClienteFinal()
        {
            var itens = new List<ItemPedido>
            {
                new ItemPedido(Guid.NewGuid(), "Produto 1", 10m, 2)
            };

            Assert.Throws<ArgumentNullException>(() =>
                new Pedido(Guid.NewGuid(), null, itens));
        }

        [Fact]
        public void Nao_Deve_Criar_Pedido_Sem_Itens()
        {
            var cliente = new ClienteFinal("João", null);

            Assert.Throws<ArgumentException>(() =>
                new Pedido(Guid.NewGuid(), cliente, null));

            Assert.Throws<ArgumentException>(() =>
                new Pedido(Guid.NewGuid(), cliente, new List<ItemPedido>()));
        }

        [Fact]
        public void Deve_Adicionar_Item_Valido()
        {
            var pedido = CriarPedidoValido();
            var novoItem = new ItemPedido(Guid.NewGuid(), "Produto 2", 5m, 1);
            pedido.AdicionarItem(novoItem);

            Assert.Contains(pedido.Itens, i => i.ProdutoId == novoItem.ProdutoId);
        }

        [Fact]
        public void Nao_Deve_Adicionar_Item_Repetido()
        {
            var pedido = CriarPedidoValido();
            var item = pedido.Itens.First();

            var ex = Assert.Throws<InvalidOperationException>(() =>
                pedido.AdicionarItem(new ItemPedido(item.ProdutoId, "Produto 1 (rep)", 12m, 1)));

            Assert.Equal("Já existe um item para este produto no pedido.", ex.Message);
        }

        [Fact]
        public void Deve_Remover_Item()
        {
            var pedido = CriarPedidoValido();
            var produtoId = pedido.Itens.First().ProdutoId;

            pedido.RemoverItem(produtoId);

            Assert.Empty(pedido.Itens);
        }

        [Fact]
        public void Nao_Deve_Remover_Item_Inexistente()
        {
            var pedido = CriarPedidoValido();

            var ex = Assert.Throws<InvalidOperationException>(() =>
                pedido.RemoverItem(Guid.NewGuid()));

            Assert.Equal("Item não encontrado.", ex.Message);
        }

        [Fact]
        public void Deve_Alterar_Status()
        {
            var pedido = CriarPedidoValido();
            pedido.AlterarStatus(StatusPedido.Finalizado);

            Assert.Equal(StatusPedido.Finalizado, pedido.Status);
        }

        [Fact]
        public void Nao_Deve_Alterar_Status_De_Pedido_Finalizado()
        {
            var pedido = CriarPedidoValido();
            pedido.AlterarStatus(StatusPedido.Finalizado);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                pedido.AlterarStatus(StatusPedido.Novo));

            Assert.Equal("Não é possível alterar um pedido finalizado.", ex.Message);
        }

        private Pedido CriarPedidoValido()
        {
            var cliente = new ClienteFinal("João", null);
            var itens = new List<ItemPedido>
            {
                new ItemPedido(Guid.NewGuid(), "Produto 1", 10m, 2)
            };
            return new Pedido(Guid.NewGuid(), cliente, itens);
        }
    }
}
