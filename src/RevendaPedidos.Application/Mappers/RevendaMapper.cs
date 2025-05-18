using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Factories;

namespace RevendaPedidos.Application.Mappers;

public static class RevendaMapper
{
    public static Revenda Map(this RevendaDto dto)
    {
        return RevendaFactory.Criar(
          dto.Cnpj,
          dto.RazaoSocial,
          dto.NomeFantasia,
          dto.Email,
          dto.Telefones,
          dto.Contatos?.Select(c => (c.Nome, c.Principal)),
          dto.EnderecosEntrega?.Select(e => (e.Nome, e.Rua, e.Numero, e.Complemento, e.Cidade, e.Estado, e.Cep))
      );
    }
    public static RevendaDto Map(this Revenda revenda)
    {
        return new RevendaDto
        {
            Cnpj = revenda.Cnpj,
            RazaoSocial = revenda.RazaoSocial,
            NomeFantasia = revenda.NomeFantasia,
            Email = revenda.Email,
            Telefones = revenda.Telefones?.Select(t => t.Numero).ToList() ?? new List<string>(),
            Contatos = revenda.Contatos?.Select(c => new ContatoDto
            {
                Nome = c.Nome,
                Principal = c.Principal
            }).ToList() ?? new List<ContatoDto>(),
            EnderecosEntrega = revenda.EnderecosEntrega?.Select(e => new EnderecoDto
            {
                Nome = e.Nome,
                Rua = e.Rua,
                Numero = e.Numero,
                Complemento = e.Complemento,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep
            }).ToList() ?? new List<EnderecoDto>()
        };
    }
}
