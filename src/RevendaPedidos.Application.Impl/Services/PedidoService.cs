using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Application.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevendaPedidos.Application.Impl.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;

        public PedidoService(IPedidoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> RegistrarPedidoAsync(PedidoDTO dto)
        {
            var entity = dto.Map(); // Mapeia DTO para entidade
            await _repository.AdicionarAsync(entity);
            return entity.Id;
        }

        public async Task<IEnumerable<PedidoDTO>> ObterPedidosPorRevendaAsync(Guid revendaId)
        {
            var entidades = await _repository.ListarPorRevendaAsync(revendaId);
            return entidades.Select(e => e.Map());
        }

        public async Task<PedidoDTO?> ObterPorIdAsync(Guid revendaId, Guid pedidoId)
        {
            var entidade = await _repository.ObterPorIdAsync(revendaId, pedidoId);
            return entidade is null ? null : entidade.Map();
        }

        public async Task<IEnumerable<PedidoDTO>> EmitirPedidosParaFornecedorAsync(Guid revendaId, List<Guid> pedidoIds)
        {
            var pedidos = await _repository.ListarPorIdsAsync(revendaId, pedidoIds);

            // Validar se encontrou todos, se pertencem à revenda
            if (pedidos.Count != pedidoIds.Count)
                throw new InvalidOperationException("Um ou mais pedidos não encontrados para a revenda.");

            // Valida cada pedido individualmente
            foreach (var pedido in pedidos)
            {
                if (!pedido.PodeEmitir())
                    throw new InvalidOperationException($"Pedido {pedido.Id} não está apto para emissão.");
            }

            // Verifica soma mínima de itens
            var somaItens = pedidos.Sum(p => p.Itens.Sum(i => i.Quantidade));
            if (somaItens < 1000)
                throw new InvalidOperationException("A soma das quantidades dos itens deve ser no mínimo 1000.");

            // Atualiza status para 'AguardandoIntegracao'
            foreach (var pedido in pedidos)
            {
                pedido.AlterarStatus(Domain.Entities.StatusPedido.AguardandoIntegracao);
                await _repository.AtualizarAsync(pedido);
            }

            // Retorna lista de pedidos atualizada
            return pedidos.Select(p => p.Map()).ToList();
        }
    }
}
