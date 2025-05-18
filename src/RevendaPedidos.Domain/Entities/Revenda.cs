using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using RevendaPedidos.Shared.Utils;

namespace RevendaPedidos.Domain.Entities
{
    public class Revenda
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        private string _cnpj = string.Empty;
        private string _razaoSocial = string.Empty;
        private string _nomeFantasia = string.Empty;
        private string _email = string.Empty;

        private readonly List<Telefone> _telefones = new();
        private readonly List<Contato> _contatos = new();
        private readonly List<EnderecoEntrega> _enderecosEntrega = new();

        public Revenda(string cnpj, string razaoSocial, string nomeFantasia, string email)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Email = email;
        }

        public void Atualizar(string cnpj, string razaoSocial, string nomeFantasia, string email)
        {
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            Email = email;
        }

        public string Cnpj
        {
            get => _cnpj;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("CNPJ é obrigatório.");

                if (!Helpers.ValidarCnpj(value))
                    throw new ArgumentException("CNPJ inválido.");

                _cnpj = value;
            }
        }

        public string RazaoSocial
        {
            get => _razaoSocial;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Razão Social é obrigatória.");

                _razaoSocial = value.Trim();
            }
        }

        public string NomeFantasia
        {
            get => _nomeFantasia;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nome Fantasia é obrigatório.");

                _nomeFantasia = value.Trim();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email é obrigatório.");

                if (!Helpers.ValidarEmail(value))
                    throw new ArgumentException("Email inválido.");

                _email = value.Trim();
            }
        }

        public IReadOnlyList<Telefone> Telefones => _telefones.AsReadOnly();
        public IReadOnlyList<Contato> Contatos => _contatos.AsReadOnly();
        public IReadOnlyList<EnderecoEntrega> EnderecosEntrega => _enderecosEntrega.AsReadOnly();
        
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

    }
}
