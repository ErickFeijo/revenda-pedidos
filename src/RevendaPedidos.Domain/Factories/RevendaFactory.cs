using System;
using System.Collections.Generic;
using System.Linq;
using RevendaPedidos.Domain.Entities;

namespace RevendaPedidos.Domain.Factories
{
    public static class RevendaFactory
    {
        public static Revenda Criar(
            string cnpj,
            string razaoSocial,
            string nomeFantasia,
            string email,
            IEnumerable<string>? telefones,
            IEnumerable<(string Nome, bool Principal)>? contatos,
            IEnumerable<(string Nome, string Rua, string Numero, string Complemento, string Cidade, string Estado, string Cep)>? enderecosEntrega)
        {
            var revenda = new Revenda(cnpj, razaoSocial, nomeFantasia, email);

            if (telefones != null)
            {
                foreach (var tel in telefones)
                    revenda.AdicionarTelefone(tel);
            }

            if (contatos != null)
            {
                foreach (var c in contatos)
                    revenda.AdicionarContato(new Contato(c.Nome, c.Principal));
            }

            if (enderecosEntrega != null && enderecosEntrega.Any())
            {
                foreach (var e in enderecosEntrega)
                {
                    var endereco = new EnderecoEntrega(e.Nome, e.Rua, e.Numero, e.Complemento, e.Cidade, e.Estado, e.Cep);
                    revenda.AdicionarEnderecoEntrega(endereco);
                }
            }
            else
            {
                throw new Exception("Deve haver ao menos um endereço de entrega.");
            }

            return revenda;
        }
    }
}
