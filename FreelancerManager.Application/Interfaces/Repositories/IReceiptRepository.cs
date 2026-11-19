using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Interfaces.Repositories;

/// <summary>
/// Contrato de persistência do agregado <see cref="Receipt"/>.
/// A implementação concreta fica em Infrastructure (Dependency Inversion).
/// </summary>
public interface IReceiptRepository
{
    Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default);

    Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Receipt>> GetAllAsync(CancellationToken cancellationToken = default);

    void Update(Receipt receipt);
}