using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Application.Services;
using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Services;

public sealed class ReceiptService(
    IReceiptRepository repository,
    ITaxCalculationStrategy taxStrategy) : IReceiptService
{
    public async Task<ReceiptResponseDto> CreateAsync(CreateReceiptRequestDto request, CancellationToken cancellationToken = default)
    {
        // 1. A Application calcula o imposto usando a estratégia escolhida
        var taxDeduction = taxStrategy.CalculateDeduction(request.GrossValue, request.CustomTaxPercentage);

        // 2. A Application instancia o Domain (O Domain valida e calcula o NetValue)
        var receipt = new Receipt(
            request.ContractId,
            request.ReceiptNumber,
            request.GrossValue,
            taxDeduction,
            request.PaymentMethod,
            DateTime.UtcNow);

        // 3. Persiste
        await repository.AddAsync(receipt, cancellationToken);

        return MapToDto(receipt);
    }

    public async Task<ReceiptResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var receipt = await repository.GetByIdAsync(id, cancellationToken);
        return receipt is null ? null : MapToDto(receipt);
    }

    public async Task<IReadOnlyList<ReceiptResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var receipts = await repository.GetAllAsync(cancellationToken);
        return receipts.Select(MapToDto).ToList();
    }

    private static ReceiptResponseDto MapToDto(Receipt receipt)
    {
        return new ReceiptResponseDto(
            receipt.Id,
            receipt.ContractId,
            receipt.ReceiptNumber,
            receipt.GrossValue,
            receipt.TaxDeductions,
            receipt.NetValue,
            receipt.PaymentMethod,
            receipt.IssuedAt);
    }
}

// Implementação padrão da Estratégia
public sealed class StandardTaxCalculationStrategy : ITaxCalculationStrategy
{
    public decimal CalculateDeduction(decimal grossValue, decimal customTaxPercentage)
    {
        if (customTaxPercentage <= 0) return 0;

        // Exemplo: Se o freelancer passa 5 (para 5%), a dedução é GrossValue * 0.05
        var percentageAsDecimal = customTaxPercentage / 100m;
        return grossValue * percentageAsDecimal;
    }
}