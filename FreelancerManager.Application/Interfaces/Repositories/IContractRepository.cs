using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Interfaces.Repositories;

public interface IContractRepository
{
    Task AddAsync(Contract contract, CancellationToken cancellationToken = default);
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Contract>> GetAllAsync(CancellationToken cancellationToken = default);
}