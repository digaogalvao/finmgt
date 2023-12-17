using FinancialManagement.Domain.Entities;
using FinancialManagement.Infrastructure.Context;
using FinancialManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinancialManagement.Tests
{
    public class TransacaoRepositoryTests
    {
        [Fact]
        public async Task GetTransacoes_ShouldReturnTransacoes()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTransacoes_ShouldReturnTransacoes")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            // Act
            var result = await repository.GetTransacoes();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Assuming the in-memory database is initially empty
        }

        [Fact]
        public async Task GetTransacao_ShouldReturnTransacaoById()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetTransacao_ShouldReturnTransacaoById")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            // Seed the in-memory database with a sample Transacao
            var transacao = new Transacao { Id = 1, Data = DateTime.Now, Descricao = "Test Transacao", Valor = 100.00m };
            dbContext.Transacoes.Add(transacao);
            dbContext.SaveChanges();

            // Act
            var result = await repository.GetTransacao(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transacao.Id, result.Id);
        }

        [Fact]
        public async Task CreateTransacao_ShouldCreateAndReturnId()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateTransacao_ShouldCreateAndReturnId")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            var transacao = new Transacao { Data = DateTime.Now, Descricao = "Test Transacao", Valor = 100.00m };

            // Act
            var result = await repository.CreateTransacao(transacao);

            // Assert
            Assert.NotEqual(0, result);
            Assert.Equal(transacao.Id, result);
        }

        [Fact]
        public async Task UpdateTransacao_ShouldUpdateTransacao()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateTransacao_ShouldUpdateTransacao")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            var transacao = new Transacao { Id = 1, Data = DateTime.Now, Descricao = "Test Transacao", Valor = 100.00m };
            dbContext.Transacoes.Add(transacao);
            dbContext.SaveChanges();

            // Modify some properties
            transacao.Descricao = "Updated Transacao";
            transacao.Valor = 150.00m;

            // Act
            await repository.UpdateTransacao(transacao);

            // Assert
            var updatedTransacao = dbContext.Transacoes.Find(1);
            Assert.NotNull(updatedTransacao);
            Assert.Equal(transacao.Descricao, updatedTransacao.Descricao);
            Assert.Equal(transacao.Valor, updatedTransacao.Valor);
        }

        [Fact]
        public async Task DeleteTransacao_ShouldDeleteTransacao()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteTransacao_ShouldDeleteTransacao")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            var transacao = new Transacao { Id = 1, Data = DateTime.Now, Descricao = "Test Transacao", Valor = 100.00m };
            dbContext.Transacoes.Add(transacao);
            dbContext.SaveChanges();

            // Act
            await repository.DeleteTransacao(transacao);

            // Assert
            var deletedTransacao = dbContext.Transacoes.Find(1);
            Assert.Null(deletedTransacao);
        }

        [Fact]
        public async Task RelatorioDiario_ShouldReturnTransacoesForGivenDate()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "RelatorioDiario_ShouldReturnTransacoesForGivenDate")
                .Options;

            using var dbContext = new AppDbContext(dbContextOptions);
            var repository = new TransacaoRepository(dbContext);

            var today = DateTime.Now.Date;
            var transacao1 = new Transacao { Id = 1, Data = today, Descricao = "Test Transacao 1", Valor = 50.00m };
            var transacao2 = new Transacao { Id = 2, Data = today.AddDays(-1), Descricao = "Test Transacao 2", Valor = 30.00m };

            dbContext.Transacoes.Add(transacao1);
            dbContext.Transacoes.Add(transacao2);
            dbContext.SaveChanges();

            // Act
            var result = await repository.RelatorioDiario(today);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count()); // Only transactions for today should be returned
            Assert.Contains(result, t => t.Id == 1);
        }
    }

}
