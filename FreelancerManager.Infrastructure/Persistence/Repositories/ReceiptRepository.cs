using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.Infrastructure.Persistence.Repositories;

public sealed class ReceiptRepository(FreelancerManagerDbContext context) : IReceiptRepository
{
    public async Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
    {
        await context.Receipts.AddAsync(receipt, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Receipts.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Receipt>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Receipts.AsNoTracking().ToListAsync(cancellationToken);
    }

    public void Update(Receipt receipt)
    {
        context.Receipts.Update(receipt);
        context.SaveChanges();
    }

    // O MÉTODO DELETE PRECISA ESTAR AQUI DENTRO E COM "public"
    public void Delete(Receipt receipt)
    {
        context.Receipts.Remove(receipt);
        context.SaveChanges();
    }
}