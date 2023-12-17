using FinancialManagement.Application.Services;
using FinancialManagement.Domain.Entities;
using FinancialManagement.Domain.Enum;
using FinancialManagement.Domain.Interfaces;
using Moq;

namespace FinancialManagement.Tests
{
    public class TransacaoServiceTests
    {
        [Fact]
        public async Task GetTransacoes_ShouldReturnOrderedTransacoes()
        {
            // Arrange
            var transacoes = new List<Transacao>
            {
                new Transacao { Id = 1, Data = DateTime.Now.AddDays(-1) },
                new Transacao { Id = 2, Data = DateTime.Now },
                new Transacao { Id = 3, Data = DateTime.Now.AddDays(-2) }
            };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.GetTransacoes()).ReturnsAsync(transacoes);

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            var result = await transacaoService.GetTransacoes();

            // Assert
            Assert.Equal(transacoes.OrderByDescending(t => t.Data), result);
        }

        [Fact]
        public async Task GetTransacao_ShouldReturnTransacaoById()
        {
            // Arrange
            var transacaoId = 1;
            var transacao = new Transacao { Id = transacaoId, Data = DateTime.Now };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.GetTransacao(transacaoId)).ReturnsAsync(transacao);

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            var result = await transacaoService.GetTransacao(transacaoId);

            // Assert
            Assert.Equal(transacao, result);
        }

        [Fact]
        public async Task CreateTransacao_ShouldReturnTransacaoId()
        {
            // Arrange
            var transacaoToCreate = new Transacao
            {
                Data = DateTime.Now,
                Descricao = "Test Transacao",
                Valor = 100.00m,
                Tipo = EnumTipoTransacao.Credito
            };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.CreateTransacao(It.IsAny<Transacao>())).ReturnsAsync(1);

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            var result = await transacaoService.CreateTransacao(transacaoToCreate);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task UpdateTransacao_ShouldUpdateTransacao()
        {
            // Arrange
            var transacaoId = 1;
            var transacaoToUpdate = new Transacao
            {
                Id = transacaoId,
                Data = DateTime.Now,
                Descricao = "Updated Transacao",
                Valor = 150.00m,
                Tipo = EnumTipoTransacao.Debito
            };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.UpdateTransacao(It.IsAny<Transacao>()));

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            await transacaoService.UpdateTransacao(transacaoId, transacaoToUpdate);

            // Assert
            transacaoRepositoryMock.Verify(repo => repo.UpdateTransacao(transacaoToUpdate), Times.Once);
        }

        [Fact]
        public async Task DeleteTransacao_ShouldDeleteTransacao()
        {
            // Arrange
            var transacaoId = 1;
            var transacaoToDelete = new Transacao { Id = transacaoId, Data = DateTime.Now };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.GetTransacao(transacaoId)).ReturnsAsync(transacaoToDelete);
            transacaoRepositoryMock.Setup(repo => repo.DeleteTransacao(It.IsAny<Transacao>()));

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            await transacaoService.DeleteTransacao(transacaoId);

            // Assert
            transacaoRepositoryMock.Verify(repo => repo.DeleteTransacao(transacaoToDelete), Times.Once);
        }

        [Fact]
        public async Task RelatorioDiario_ShouldReturnRelatorioDiario()
        {
            // Arrange
            var data = DateTime.Now;
            var transacoesDoDia = new List<Transacao>
            {
                new Transacao { Id = 1, Data = data, Valor = 100.00m, Tipo = EnumTipoTransacao.Credito },
                new Transacao { Id = 2, Data = data, Valor = 50.00m, Tipo = EnumTipoTransacao.Debito },
                new Transacao { Id = 3, Data = data.AddDays(-1), Valor = 30.00m, Tipo = EnumTipoTransacao.Credito }
            };

            var transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            transacaoRepositoryMock.Setup(repo => repo.RelatorioDiario(data)).ReturnsAsync(transacoesDoDia);

            var transacaoService = new TransacaoService(transacaoRepositoryMock.Object);

            // Act
            var result = await transacaoService.RelatorioDiario(data);

            // Assert
            Assert.Equal(data, result.Data);
            Assert.Equal(80.00m, result.SaldoDoDia);
        }
    }
}
