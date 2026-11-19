using FreelancerManager.Application.DTOs;

namespace FreelancerManager.Application.Services;

public interface IReceiptService
{
    Task<ReceiptResponseDto> CreateAsync(CreateReceiptRequestDto request, CancellationToken cancellationToken = default);
    Task<ReceiptResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ReceiptResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdateReceiptRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

// Padrão Strategy: Permite trocar a regra de cálculo de imposto no futuro (ex: Simples Nacional, MEI)
public interface ITaxCalculationStrategy
{
    decimal CalculateDeduction(decimal grossValue, decimal customTaxPercentage);
}