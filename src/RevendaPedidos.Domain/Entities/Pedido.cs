using System;
using System.Collections.Generic;
using System.Linq;

namespace RevendaPedidos.Domain.Entities
{
    public enum StatusPedido
    {
        Novo,
        AguardandoIntegracao,
        Finalizado,
        FalhaIntegracao,
        Cancelado
    }

    public class Pedido
    {
        public Guid Id { get; private set; }
        public Guid RevendaId { get; private set; }
        public ClienteFinal ClienteFinal { get; private set; }
        public DateTime DataCriacao { get; private set; }
        private readonly List<ItemPedido> _itens = new();
        public IReadOnlyCollection<ItemPedido> Itens => _itens.AsReadOnly();
        public StatusPedido Status { get; private set; }

        private Pedido() { }

        public Pedido(Guid revendaId, ClienteFinal clienteFinal, List<ItemPedido> itens)
        {
            if (revendaId == Guid.Empty)
                throw new ArgumentException("RevendaId não pode ser vazio.");
            if (clienteFinal == null)
                throw new ArgumentNullException(nameof(clienteFinal));
            if (itens == null || !itens.Any())
                throw new ArgumentException("Pedido deve possuir ao menos um ItemPedido.");

            Id = Guid.NewGuid();
            RevendaId = revendaId;
            ClienteFinal = new ClienteFinal(clienteFinal.Nome, clienteFinal.Documento);
            DataCriacao = DateTime.Now;
            _itens.AddRange(itens);
            Status = StatusPedido.Novo;
        }

        public void AdicionarItem(ItemPedido item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (_itens.Any(i => i.ProdutoId == item.ProdutoId))
                throw new InvalidOperationException("Já existe um item para este produto no pedido.");
            _itens.Add(item);
        }

        public void RemoverItem(Guid produtoId)
        {
            var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item == null)
                throw new InvalidOperationException("Item não encontrado.");

            _itens.Remove(item);
        }

        public void AlterarStatus(StatusPedido novoStatus)
        {
            if (Status == StatusPedido.Finalizado && novoStatus != StatusPedido.Finalizado)
                throw new InvalidOperationException("Não é possível alterar um pedido finalizado.");
            Status = novoStatus;
        }

        public bool PodeEmitir()
        {
            return (Status == StatusPedido.Novo || Status == StatusPedido.FalhaIntegracao)
                && _itens.Any();
        }

    }
}
