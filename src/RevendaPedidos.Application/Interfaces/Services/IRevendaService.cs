using RevendaPedidos.Application.DTOs;

namespace RevendaPedidos.Application.Interfaces.Services;

public interface IRevendaService
{
    Task<Guid> CadastrarRevendaAsync(RevendaDto dto);
    Task<IEnumerable<RevendaDto>> ObterTodasAsync();
    Task<RevendaDto?> ObterPorIdAsync(Guid id);
    Task<bool> AtualizarRevendaAsync(Guid id, RevendaDto dto);
    Task<bool> RemoverRevendaAsync(Guid id);
}
