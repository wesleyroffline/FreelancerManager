using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.Infrastructure.Persistence.Repositories;

public sealed class ClientRepository(FreelancerManagerDbContext context) : IClientRepository
{
    public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
    {
        await context.Clients.AddAsync(client, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clients.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clients.AsNoTracking().ToListAsync(cancellationToken);
    }

    public void Delete(Client client)
    {
        context.Clients.Remove(client);
        context.SaveChanges();
    }
}