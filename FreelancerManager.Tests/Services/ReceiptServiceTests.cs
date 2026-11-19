using FluentAssertions;
using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Application.Services;
using FreelancerManager.Domain.Entities;
using Moq;
using Xunit;

namespace FreelancerManager.Tests.Services;

public class ReceiptServiceTests
{
    private readonly Mock<IReceiptRepository> _repositoryMock;
    private readonly Mock<ITaxCalculationStrategy> _taxStrategyMock;
    private readonly ReceiptService _service;

    public ReceiptServiceTests()
    {
        // Configurando os "dublês" (Mocks)
        _repositoryMock = new Mock<IReceiptRepository>();
        _taxStrategyMock = new Mock<ITaxCalculationStrategy>();

        // Instanciando o Service real, mas passando as dependências falsas
        _service = new ReceiptService(_repositoryMock.Object, _taxStrategyMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Calculate_Tax_Call_Repository_And_Return_Dto()
    {
        // Arrange (Preparação)
        var request = new CreateReceiptRequestDto(
            Guid.NewGuid(), "REC-123", 1000m, 10m, "PIX");

        // Simulando a resposta da Estratégia de Imposto (Fingindo que o imposto deu 100)
        _taxStrategyMock
            .Setup(x => x.CalculateDeduction(request.GrossValue, request.CustomTaxPercentage))
            .Returns(100m);

        // Act (Ação)
        var result = await _service.CreateAsync(request);

        // Assert (Verificação)

        // 1. O Service calculou e devolveu o DTO certo?
        result.Should().NotBeNull();
        result.GrossValue.Should().Be(1000m);
        result.TaxDeductions.Should().Be(100m);
        result.NetValue.Should().Be(900m);
        result.ReceiptNumber.Should().Be("REC-123");

        // 2. O Service mandou salvar no banco? (Verificando a chamada do Mock)
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Receipt>(), It.IsAny<CancellationToken>()), Times.Once);

        // 3. O Service usou a estratégia de imposto? (Verificando a chamada do Mock)
        _taxStrategyMock.Verify(x => x.CalculateDeduction(1000m, 10m), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Receipt_Not_Found()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Simulando que o banco retornou null (recibo não existe)
        _repositoryMock
            .Setup(x => x.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Receipt?)null);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }
}