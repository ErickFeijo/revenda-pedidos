using RevendaPedidos.Domain.Entities;

namespace RevendaPedidos.Domain.Interfaces;

public interface IRevendaRepository
{
    Task AdicionarAsync(Revenda revenda);
    Task<Revenda?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Revenda>> ListarTodasAsync();
    Task AtualizarAsync(Revenda revenda);
    Task RemoverAsync(Revenda revenda);
}
