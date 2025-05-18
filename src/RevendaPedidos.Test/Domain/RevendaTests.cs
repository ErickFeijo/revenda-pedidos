using System;
using System.Linq;
using RevendaPedidos.Domain.Entities;
using Xunit;

namespace RevendaPedidos.Tests.Domain
{
    public class RevendaTests
    {
        [Fact]
        public void CriarRevenda_DeveSerValido()
        {
            var revenda = new Revenda("11222333000181", "Empresa Ltda", "Empresa", "teste@email.com");

            Assert.Equal("11222333000181", revenda.Cnpj);
            Assert.Equal("Empresa Ltda", revenda.RazaoSocial);
            Assert.Equal("Empresa", revenda.NomeFantasia);
            Assert.Equal("teste@email.com", revenda.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        public void CriarRevenda_ComCnpjInvalido_DeveLancarExcecao(string cnpjInvalido)
        {
            Assert.Throws<ArgumentException>(() =>
                new Revenda(cnpjInvalido, "Empresa Ltda", "Empresa", "teste@email.com"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("email_invalido")]
        public void CriarRevenda_ComEmailInvalido_DeveLancarExcecao(string email)
        {
            Assert.Throws<ArgumentException>(() =>
                new Revenda("11222333000181", "Empresa Ltda", "Empresa", email));
        }

        [Fact]
        public void AdicionarTelefone_Valido_DeveAdicionar()
        {
            var revenda = CriarRevendaValida();
            revenda.AdicionarTelefone("51999999999");

            Assert.Contains(revenda.Telefones, t => t.Numero == "51999999999");
        }

        [Fact]
        public void AdicionarTelefone_Repetido_DeveLancarExcecao()
        {
            var revenda = CriarRevendaValida();
            revenda.AdicionarTelefone("51999999999");

            var ex = Assert.Throws<InvalidOperationException>(() =>
                revenda.AdicionarTelefone("51999999999"));

            Assert.Equal("Telefone já adicionado.", ex.Message);
        }

        [Fact]
        public void AdicionarContato_DeveAdicionar()
        {
            var revenda = CriarRevendaValida();
            var contato = new Contato("João", true);

            revenda.AdicionarContato(contato);

            Assert.Single(revenda.Contatos);
            Assert.True(revenda.Contatos.First().Principal);
        }

        [Fact]
        public void AdicionarContato_MaisDeUmPrincipal_DeveLancarExcecao()
        {
            var revenda = CriarRevendaValida();
            revenda.AdicionarContato(new Contato("João", true));

            var ex = Assert.Throws<InvalidOperationException>(() =>
                revenda.AdicionarContato(new Contato("Maria", true)));

            Assert.Equal("Já existe um contato principal.", ex.Message);
        }

        [Fact]
        public void AdicionarEndereco_DeveAdicionar()
        {
            var revenda = CriarRevendaValida();
            var endereco = new EnderecoEntrega("Loja 1", "Rua 1", "123", "Casa", "Porto Alegre", "RS", "90000-000");

            revenda.AdicionarEnderecoEntrega(endereco);

            Assert.Single(revenda.EnderecosEntrega);
        }

        [Fact]
        public void RemoverUltimoEndereco_DeveLancarExcecao()
        {
            var revenda = CriarRevendaValida();
            var endereco = new EnderecoEntrega("Loja 1", "Rua 1", "123", "Casa", "Porto Alegre", "RS", "90000-000");
            revenda.AdicionarEnderecoEntrega(endereco);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                revenda.RemoverEnderecoEntrega(endereco));

            Assert.Equal("Deve haver pelo menos um endereço de entrega.", ex.Message);
        }

        private Revenda CriarRevendaValida()
        {
            return new Revenda("11222333000181", "Empresa Ltda", "Empresa", "teste@email.com");
        }
    }
}
