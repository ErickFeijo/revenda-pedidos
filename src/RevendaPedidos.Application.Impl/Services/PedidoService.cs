using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Application.Mappers;

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

    }
}
