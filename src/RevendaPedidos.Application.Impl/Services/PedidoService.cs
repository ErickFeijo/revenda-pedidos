using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Application.Mappers;
using RevendaPedidos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RevendaPedidos.Application.Impl.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly IFilaProcessarPedidosService _filaProcessarPedidosService;

        public PedidoService(IPedidoRepository repository, IFilaProcessarPedidosService filaProcessarPedidosService)
        {
            _repository = repository;
            _filaProcessarPedidosService = filaProcessarPedidosService;
        }

        public async Task<Guid> RegistrarPedidoAsync(PedidoDTO dto)
        {
            var entity = dto.Map();
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

            if (pedidos.Count != pedidoIds.Count)
                throw new InvalidOperationException("Um ou mais pedidos não encontrados para a revenda.");

            foreach (var pedido in pedidos)
            {
                if (!pedido.PodeEmitir())
                    throw new InvalidOperationException($"Pedido {pedido.Id} não está apto para emissão.");
            }

            var somaItens = pedidos.Sum(p => p.Itens.Sum(i => i.Quantidade));
            if (somaItens < 1000)
                throw new InvalidOperationException("A soma das quantidades dos itens deve ser no mínimo 1000.");

            // Enviar para a fila de processamento
            foreach (var pedido in pedidos)
            {
                await _filaProcessarPedidosService.PublicarPedidoAsync(pedido.MapFila());

                pedido.AlterarStatus(StatusPedido.AguardandoIntegracao);
                
                await _repository.AtualizarAsync(pedido);
            }

            return pedidos.Select(p => p.Map()).ToList();
        }
    }
}
