using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Api.Models.Requests;

namespace RevendaPedidos.Api.Mappers;

public static class RevendaMapper
{
    public static RevendaDto Map(this RevendaRequest request)
    {
        return new RevendaDto
        {
            Cnpj = request.Cnpj,
            RazaoSocial = request.RazaoSocial,
            NomeFantasia = request.NomeFantasia,
            Email = request.Email,
            Telefones = request.Telefones?.ToList() ?? new List<string>(),
            Contatos = request.Contatos?.Select(c => new ContatoDto
            {
                Nome = c.Nome,
                Principal = c.Principal
            }).ToList() ?? new List<ContatoDto>(),
            EnderecosEntrega = request.EnderecosEntrega?.Select(e => new EnderecoDto
            {
                Nome = e.Nome,
                Rua = e.Rua,
                Numero = e.Numero,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep
            }).ToList() ?? new List<EnderecoDto>()
        };
    }
}
