using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;
using FreelancerManager.Domain.Exceptions;

namespace FreelancerManager.Application.Services;

public sealed class ReceiptService(
    IReceiptRepository repository,
    ITaxCalculationStrategy taxStrategy) : IReceiptService
{
    public async Task<ReceiptResponseDto> CreateAsync(CreateReceiptRequestDto request, CancellationToken cancellationToken = default)
    {
        var taxDeduction = taxStrategy.CalculateDeduction(request.GrossValue, request.CustomTaxPercentage);

        var receipt = new Receipt(
            request.ContractId,
            request.ReceiptNumber,
            request.GrossValue,
            taxDeduction,
            request.PaymentMethod,
            DateTime.UtcNow);

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

    // --- MÉTODOS NOVOS ADICIONADOS AQUI DENTRO ---
    public async Task UpdateAsync(Guid id, UpdateReceiptRequestDto request, CancellationToken cancellationToken = default)
    {
        var receipt = await repository.GetByIdAsync(id, cancellationToken)
            ?? throw new DomainException("Recibo não encontrado.");

        var taxDeduction = taxStrategy.CalculateDeduction(request.GrossValue, request.CustomTaxPercentage);

        receipt.Update(request.GrossValue, taxDeduction, request.PaymentMethod);

        repository.Update(receipt);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var receipt = await repository.GetByIdAsync(id, cancellationToken);
        if (receipt is null) return false;

        repository.Delete(receipt);
        return true;
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

// --- CLASSE DA ESTRATÉGIA DE IMPOSTO ---
public sealed class StandardTaxCalculationStrategy : ITaxCalculationStrategy
{
    public decimal CalculateDeduction(decimal grossValue, decimal customTaxPercentage)
    {
        if (customTaxPercentage <= 0) return 0;

        var percentageAsDecimal = customTaxPercentage / 100m;
        return grossValue * percentageAsDecimal;
    }
}