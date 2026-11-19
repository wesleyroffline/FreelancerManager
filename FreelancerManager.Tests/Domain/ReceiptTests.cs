using FreelancerManager.Domain.Entities;
using FreelancerManager.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace FreelancerManager.Tests.Domain;

public class ReceiptTests
{
    // --- TESTES DE SUCESSO (Happy Path) ---

    [Fact]
    public void Constructor_Should_Calculate_NetValue_Correctly()
    {
        // Arrange (Preparação)
        var contractId = Guid.NewGuid();
        var grossValue = 1000m;
        var taxDeductions = 150m;

        // Act (Ação)
        var receipt = new Receipt(
            contractId,
            "REC-001",
            grossValue,
            taxDeductions,
            "PIX",
            DateTime.UtcNow);

        // Assert (Verificação)
        receipt.NetValue.Should().Be(850m);
        receipt.GrossValue.Should().Be(grossValue);
        receipt.TaxDeductions.Should().Be(taxDeductions);
    }

    // --- TESTES DE EXCEÇÃO (Guard Clauses) ---

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_ContractId_Is_Empty()
    {
        // Act
        Action act = () => new Receipt(
            Guid.Empty, "REC-001", 1000m, 100m, "PIX", DateTime.UtcNow);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*ContractId*");
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_ReceiptNumber_Is_Empty()
    {
        // Act
        Action act = () => new Receipt(
            Guid.NewGuid(), "   ", 1000m, 100m, "PIX", DateTime.UtcNow);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*número do recibo*");
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_GrossValue_Is_Zero_Or_Negative()
    {
        // Act
        Action act = () => new Receipt(
            Guid.NewGuid(), "REC-001", -50m, 0m, "PIX", DateTime.UtcNow);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*valor bruto*");
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_TaxDeductions_Exceed_GrossValue()
    {
        // Act (Tentando deduzir 1500 de um valor bruto de 1000)
        Action act = () => new Receipt(
            Guid.NewGuid(), "REC-001", 1000m, 1500m, "PIX", DateTime.UtcNow);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*maiores que o valor bruto*");
    }

    [Fact]
    public void Constructor_Should_Throw_DomainException_When_PaymentMethod_Is_Empty()
    {
        // Act
        Action act = () => new Receipt(
            Guid.NewGuid(), "REC-001", 1000m, 100m, "", DateTime.UtcNow);

        // Assert
        act.Should().Throw<DomainException>()
           .WithMessage("*forma de pagamento*");
    }
}