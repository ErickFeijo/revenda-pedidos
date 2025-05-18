using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.Application.Impl.Services;
using RevendaPedidos.Domain.Entities;
using RevendaPedidos.Domain.Interfaces;
using Xunit;

namespace RevendaPedidos.Tests.Application
{
    public class RevendaServiceTests
    {
        private readonly Mock<IRevendaRepository> _repoMock;
        private readonly RevendaService _service;

        public RevendaServiceTests()
        {
            _repoMock = new Mock<IRevendaRepository>();
            _service = new RevendaService(_repoMock.Object);
        }

        [Fact]
        public async Task CadastrarRevendaAsync_DeveChamarRepositorioERetornarId()
        {
            var dto = new RevendaDto
            {
                Cnpj = "11222333000181",
                RazaoSocial = "Teste",
                NomeFantasia = "Fantasia",
                Email = "a@a.com",
                EnderecosEntrega = new List<EnderecoDto>
                {
                    new EnderecoDto
                    {
                        Nome = "Matriz",
                        Rua = "Rua A",
                        Numero = "123",
                        Complemento = "Sala 1",
                        Cidade = "Cidade",
                        Estado = "SP",
                        Cep = "01001-000"
                    }
                }
            };

            _repoMock.Setup(r => r.AdicionarAsync(It.IsAny<Revenda>()))
                     .Callback<Revenda>(r => r.GetType().GetProperty("Id")?.SetValue(r, Guid.NewGuid()))
                     .Returns(Task.CompletedTask);

            var id = await _service.CadastrarRevendaAsync(dto);

            _repoMock.Verify(r => r.AdicionarAsync(It.IsAny<Revenda>()), Times.Once);
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public async Task CadastrarRevendaAsync_SemEnderecoEntrega_DeveLancarExcecao()
        {
            var dto = new RevendaDto
            {
                Cnpj = "11222333000181",
                RazaoSocial = "Teste",
                NomeFantasia = "Fantasia",
                Email = "a@a.com",
                EnderecosEntrega = new List<EnderecoDto>()
            };

            await Assert.ThrowsAsync<Exception>(() => _service.CadastrarRevendaAsync(dto));
        }


        [Fact]
        public async Task ObterTodasAsync_DeveRetornarDtosMapeados()
        {
            var cnpj = "11222333000181";
            var revendas = new List<Revenda> { new Revenda(cnpj, "Razao", "Fantasia", "a@a.com") };
            _repoMock.Setup(r => r.ListarTodasAsync()).ReturnsAsync(revendas);

            var result = await _service.ObterTodasAsync();

            // Opção 1: Usando ToList()
            var listaResult = result.ToList();
            Assert.Single(listaResult); // Garante que só veio 1
            Assert.Equal(cnpj, listaResult[0].Cnpj);

            // Opção 2: Usando Single()
            var dto = result.Single();
            Assert.Equal(cnpj, dto.Cnpj);
        }


        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoEncontrado()
        {
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Revenda?)null);

            var result = await _service.ObterPorIdAsync(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AtualizarRevendaAsync_DeveRetornarFalse_QuandoNaoEncontrado()
        {
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Revenda?)null);

            var result = await _service.AtualizarRevendaAsync(Guid.NewGuid(), new RevendaDto());

            Assert.False(result);
        }

        [Fact]
        public async Task AtualizarRevendaAsync_DeveAtualizar_QuandoEncontrado()
        {
            var entity = new Revenda("11222333000181", "Razao", "Fantasia", "a@a.com");
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            _repoMock.Setup(r => r.AtualizarAsync(It.IsAny<Revenda>())).Returns(Task.CompletedTask);

            var result = await _service.AtualizarRevendaAsync(Guid.NewGuid(), new RevendaDto
            {
                Cnpj = "11222333000181",
                RazaoSocial = "Nova",
                NomeFantasia = "NovaF",
                Email = "b@b.com"
            });

            Assert.True(result);
            _repoMock.Verify(r => r.AtualizarAsync(It.IsAny<Revenda>()), Times.Once);
        }

        [Fact]
        public async Task RemoverRevendaAsync_DeveRetornarFalse_QuandoNaoEncontrado()
        {
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Revenda?)null);

            var result = await _service.RemoverRevendaAsync(Guid.NewGuid());

            Assert.False(result);
        }

        [Fact]
        public async Task RemoverRevendaAsync_DeveRemover_QuandoEncontrado()
        {
            var entity = new Revenda("11222333000181", "Razao", "Fantasia", "a@a.com");
            _repoMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(entity);
            _repoMock.Setup(r => r.RemoverAsync(It.IsAny<Revenda>())).Returns(Task.CompletedTask);

            var result = await _service.RemoverRevendaAsync(Guid.NewGuid());

            Assert.True(result);
            _repoMock.Verify(r => r.RemoverAsync(It.IsAny<Revenda>()), Times.Once);
        }
    }
}
