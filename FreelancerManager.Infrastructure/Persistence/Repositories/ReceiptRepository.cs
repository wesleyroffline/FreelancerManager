using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.Infrastructure.Persistence.Repositories;

public sealed class ReceiptRepository(FreelancerManagerDbContext context) : IReceiptRepository
{
	public async Task AddAsync(Receipt receipt, CancellationToken cancellationToken = default)
	{
		await context.Receipts.AddAsync(receipt, cancellationToken);
		// Nota: Em Clean Architecture rigorosa, o SaveChanges fica no Unit of Work.
		// Para simplificar a primeira versão, salvamos aqui.
		await context.SaveChangesAsync(cancellationToken);
	}

	public async Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await context.Receipts.FindAsync([id], cancellationToken);
	}

	public async Task<IReadOnlyList<Receipt>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		// AsNoTracking melhora a performance de leitura, pois o EF não precisa monitorar mudanças.
		return await context.Receipts.AsNoTracking().ToListAsync(cancellationToken);
	}

	public void Update(Receipt receipt)
	{
		context.Receipts.Update(receipt);
	}
}