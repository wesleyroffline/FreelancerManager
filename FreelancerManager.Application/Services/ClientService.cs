using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Services;

public sealed class ClientService(IClientRepository repository) : IClientService
{
    public async Task<ClientResponseDto> CreateAsync(CreateClientRequestDto request, CancellationToken cancellationToken = default)
    {
        var client = new Client(request.Name, request.Email, request.Document);
        await repository.AddAsync(client, cancellationToken);

        return new ClientResponseDto(client.Id, client.Name, client.Email, client.Document);
    }

    public async Task<ClientResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await repository.GetByIdAsync(id, cancellationToken);
        return client is null ? null : new ClientResponseDto(client.Id, client.Name, client.Email, client.Document);
    }

    public async Task<IReadOnlyList<ClientResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var clients = await repository.GetAllAsync(cancellationToken);
        return clients.Select(c => new ClientResponseDto(c.Id, c.Name, c.Email, c.Document)).ToList();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await repository.GetByIdAsync(id, cancellationToken);
        if (client is null) return false;

        repository.Delete(client);
        return true;
    }
}