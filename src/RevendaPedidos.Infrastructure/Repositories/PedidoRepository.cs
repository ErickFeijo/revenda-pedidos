using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;

namespace RevendaPedidos.Infra.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly RevendaPedidosDbContext _context;

        public PedidoRepository(RevendaPedidosDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Pedido>> ListarPorRevendaAsync(Guid revendaId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .Where(p => p.RevendaId == revendaId)
                .OrderByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid revendaId, Guid pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.RevendaId == revendaId && p.Id == pedidoId);
        }

    }
}
