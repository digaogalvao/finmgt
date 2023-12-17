using FinancialManagement.Api.Controllers;
using FinancialManagement.Application.Interfaces;
using FinancialManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinancialManagement.Tests
{
    public class TransacoesControllerTests
    {
        [Fact]
        public async Task Index_ShouldReturnListOfTransacoes()
        {
            // Arrange
            var transacoesMock = new List<Transacao>
        {
            new Transacao { Id = 1, Descricao = "Transacao 1", Valor = 100 },
            new Transacao { Id = 2, Descricao = "Transacao 2", Valor = 150 },
        };

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.GetTransacoes()).ReturnsAsync(transacoesMock);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var transacoes = Assert.IsAssignableFrom<IEnumerable<Transacao>>(okResult.Value);
            Assert.Equal(2, transacoes.Count());
        }

        [Fact]
        public async Task Details_WithValidId_ShouldReturnTransacao()
        {
            // Arrange
            var transacaoId = 1;
            var transacaoMock = new Transacao { Id = 1, Descricao = "Transacao 1", Valor = 100 };

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.GetTransacao(transacaoId)).ReturnsAsync(transacaoMock);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Details(transacaoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var transacao = Assert.IsAssignableFrom<Transacao>(okResult.Value);
            Assert.Equal(transacaoMock.Id, transacao.Id);
        }

        [Fact]
        public async Task Details_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidTransacaoId = 99;

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.GetTransacao(invalidTransacaoId)).ReturnsAsync((Transacao)null);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Details(invalidTransacaoId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void Create_ShouldReturnOkResult()
        {
            // Arrange
            var transacaoServiceMock = new Mock<ITransacaoService>();
            var controller = new TransacoesController(transacaoServiceMock.Object);
            var transacao = new Transacao { Id = 1, Descricao = "Nova Transacao", Valor = 200 };

            // Act
            var result = controller.Create(transacao);

            // Assert
            Assert.IsType<OkResult>(result);
            transacaoServiceMock.Verify(service => service.CreateTransacao(It.IsAny<Transacao>()), Times.Once);
        }

        [Fact]
        public async Task Edit_WithValidId_ShouldReturnCreatedAtRoute()
        {
            // Arrange
            var transacaoId = 1;
            var transacaoMock = new Transacao { Id = 1, Descricao = "Transacao 1", Valor = 100 };

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.UpdateTransacao(transacaoId, It.IsAny<Transacao>())).Returns(Task.CompletedTask);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Edit(transacaoId, transacaoMock);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetTransacao", createdAtRouteResult.RouteName);
            transacaoServiceMock.Verify(service => service.UpdateTransacao(transacaoId, It.IsAny<Transacao>()), Times.Once);
        }

        [Fact]
        public async Task Edit_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidTransacaoId = 99;
            var transacaoMock = new Transacao { Id = 1, Descricao = "Transacao 1", Valor = 100 };

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.UpdateTransacao(invalidTransacaoId, It.IsAny<Transacao>())).Throws(new KeyNotFoundException());

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Edit(invalidTransacaoId, transacaoMock);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnOkResult()
        {
            // Arrange
            var transacaoId = 1;
            var transacaoMock = new Transacao { Id = 1, Descricao = "Transacao 1", Valor = 100 };

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.GetTransacao(transacaoId)).ReturnsAsync(transacaoMock);
            transacaoServiceMock.Setup(service => service.DeleteTransacao(transacaoId)).Returns(Task.CompletedTask);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Delete(transacaoId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var invalidTransacaoId = 99;

            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock.Setup(service => service.GetTransacao(invalidTransacaoId)).ReturnsAsync((Transacao)null);

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            var result = await controller.Delete(invalidTransacaoId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GerarRelatorioDiario_ShouldReturnOkResult()
        {
            // Arrange
            var transacaoServiceMock = new Mock<ITransacaoService>();
            transacaoServiceMock
                .Setup(service => service.RelatorioDiario(It.IsAny<DateTime>()))
                .ReturnsAsync(new RelatorioDiario { SaldoDoDia = 150 });

            var controller = new TransacoesController(transacaoServiceMock.Object);

            // Act
            IActionResult actionResult = await controller.GerarRelatorioDiario(DateTime.Now);
            var result = actionResult as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var relatorioDiario = Assert.IsType<RelatorioDiario>(result.Value);
            Assert.Equal(150, relatorioDiario.SaldoDoDia);
        }


    }

}