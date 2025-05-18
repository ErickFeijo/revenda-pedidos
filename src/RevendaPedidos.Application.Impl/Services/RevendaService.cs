using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Application.Mappers;

namespace RevendaPedidos.Application.Impl.Services;

public class RevendaService : IRevendaService
{
    private readonly IRevendaRepository _repository;

    public RevendaService(IRevendaRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CadastrarRevendaAsync(RevendaDto dto)
    {
        var entity = dto.Map();
        await _repository.AdicionarAsync(entity);
        return entity.Id;
    }

    public async Task<IEnumerable<RevendaDto>> ObterTodasAsync()
    {
        var entidades = await _repository.ListarTodasAsync();
        return entidades.Select(e => e.Map());
    }

    public async Task<RevendaDto?> ObterPorIdAsync(Guid id)
    {
        var entidade = await _repository.ObterPorIdAsync(id);
        return entidade is null ? null : entidade.Map();
    }

    public async Task<bool> AtualizarRevendaAsync(Guid id, RevendaDto dto)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente is null)
            return false;

        existente.Atualizar(dto.Cnpj, dto.RazaoSocial, dto.NomeFantasia, dto.Email);

        await _repository.AtualizarAsync(existente);
        return true;
    }

    public async Task<bool> RemoverRevendaAsync(Guid id)
    {
        var existente = await _repository.ObterPorIdAsync(id);
        if (existente is null)
            return false;

        await _repository.RemoverAsync(existente);
        return true;
    }
}
