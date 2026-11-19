using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.Infrastructure.Persistence.Repositories;

public sealed class ContractRepository(FreelancerManagerDbContext context) : IContractRepository
{
    public async Task AddAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        await context.Contracts.AddAsync(contract, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Contracts.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Contract>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Contracts.AsNoTracking().ToListAsync(cancellationToken);
    }
}