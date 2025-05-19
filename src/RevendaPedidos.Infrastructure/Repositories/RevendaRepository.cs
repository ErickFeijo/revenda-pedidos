using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;

namespace RevendaPedidos.Infra.Repositories;

public class RevendaRepository : IRevendaRepository
{
    private readonly RevendaPedidosDbContext _context;

    public RevendaRepository(RevendaPedidosDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Revenda revenda)
    {
        await _context.Revendas.AddAsync(revenda);
        await _context.SaveChangesAsync();
    }

    public async Task<Revenda?> ObterPorIdAsync(Guid id)
    {
        return await _context.Revendas
            .Include("_contatos")
            .Include("_enderecosEntrega")
            .Include("_telefones")
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Revenda>> ListarTodasAsync()
    {
        return await _context.Revendas
            .Include("_contatos")
            .Include("_enderecosEntrega")
            .Include("_telefones")
            .ToListAsync();
    }

    public async Task AtualizarAsync(Revenda revenda)
    {
        _context.Revendas.Update(revenda);
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(Revenda revenda)
    {
        _context.Revendas.Remove(revenda);
        await _context.SaveChangesAsync();
    }
}
