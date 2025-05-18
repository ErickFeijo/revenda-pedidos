using System;
using System.Collections.Generic;
using System.Linq;
using RevendaPedidos.Shared.Utils;

namespace RevendaPedidos.Domain.Entities
{
    public class Revenda
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Cnpj { get; private set; } = string.Empty;
        public string RazaoSocial { get; private set; } = string.Empty;
        public string NomeFantasia { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        private List<Telefone> _telefones { get; set; } = new();
        private List<Contato> _contatos { get; set; } = new();
        private List<EnderecoEntrega> _enderecosEntrega { get; set; } = new();

        public IReadOnlyList<Telefone> Telefones => _telefones.AsReadOnly();
        public IReadOnlyList<Contato> Contatos => _contatos.AsReadOnly();
        public IReadOnlyList<EnderecoEntrega> EnderecosEntrega => _enderecosEntrega.AsReadOnly();

        public Revenda(string cnpj, string razaoSocial, string nomeFantasia, string email)
        {
            Atualizar(cnpj, razaoSocial, nomeFantasia, email);
        }

        public void Atualizar(string cnpj, string razaoSocial, string nomeFantasia, string email)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                throw new ArgumentException("CNPJ é obrigatório.");

            if (!Helpers.ValidarCnpj(cnpj))
                throw new ArgumentException("CNPJ inválido.");

            if (string.IsNullOrWhiteSpace(razaoSocial))
                throw new ArgumentException("Razão Social é obrigatória.");

            if (string.IsNullOrWhiteSpace(nomeFantasia))
                throw new ArgumentException("Nome Fantasia é obrigatório.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email é obrigatório.");

            if (!Helpers.ValidarEmail(email))
                throw new ArgumentException("Email inválido.");

            Cnpj = cnpj.Trim();
            RazaoSocial = razaoSocial.Trim();
            NomeFantasia = nomeFantasia.Trim();
            Email = email.Trim();
        }

        #region Listas

        public void AdicionarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                throw new ArgumentException("Telefone inválido.");

            if (_telefones.Any(x => x.Numero == telefone))
                throw new InvalidOperationException("Telefone já adicionado.");

            _telefones.Add(new Telefone(telefone));
        }

        public void RemoverTelefone(string telefone)
        {
            _telefones.Remove(new Telefone(telefone));
        }

        public void AdicionarContato(Contato contato)
        {
            if (contato == null)
                throw new ArgumentException("Contato inválido.");

            if (contato.Principal && _contatos.Any(c => c.Principal))
                throw new InvalidOperationException("Já existe um contato principal.");

            _contatos.Add(contato);
        }

        public void RemoverContato(Contato contato)
        {
            _contatos.Remove(contato);

            if (!_contatos.Any(c => c.Principal) && _contatos.Any())
                throw new InvalidOperationException("Deve haver pelo menos um contato principal.");
        }

        public void AdicionarEnderecoEntrega(EnderecoEntrega endereco)
        {
            if (endereco == null)
                throw new ArgumentException("Endereço de entrega inválido.");

            _enderecosEntrega.Add(endereco);
        }

        public void RemoverEnderecoEntrega(EnderecoEntrega endereco)
        {
            _enderecosEntrega.Remove(endereco);

            if (!_enderecosEntrega.Any())
                throw new InvalidOperationException("Deve haver pelo menos um endereço de entrega.");
        }

        #endregion
    }
}
