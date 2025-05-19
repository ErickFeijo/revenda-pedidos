using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Include("_itens")
                .Where(p => p.RevendaId == revendaId)
                .OrderByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdAsync(Guid revendaId, Guid pedidoId)
        {
            return await _context.Pedidos
                .Include("_itens")
                .FirstOrDefaultAsync(p => p.RevendaId == revendaId && p.Id == pedidoId);
        }

        public async Task<List<Pedido>> ListarPorIdsAsync(Guid revendaId, List<Guid> pedidoIds)
        {
            return await _context.Pedidos
                .Include("_itens")
                .Where(p => p.RevendaId == revendaId && pedidoIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pedido>> ListarPorStatusAsync(StatusPedido status)
        {
            return await _context.Pedidos
                .Include("_itens")
                .Where(p => p.Status == status)
                .ToListAsync();
        }
    }
}
