using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task AddAsync(Client client, CancellationToken cancellationToken = default);
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Client>> GetAllAsync(CancellationToken cancellationToken = default);

    void Delete(Client client);
}