using FreelancerManager.Application.DTOs;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Domain.Entities;

namespace FreelancerManager.Application.Services;

public sealed class ContractService(IContractRepository repository) : IContractService
{
    public async Task<ContractResponseDto> CreateAsync(CreateContractRequestDto request, CancellationToken cancellationToken = default)
    {
        var contract = new Contract(request.ClientId, request.Title, request.TotalValue, request.StartDate);
        await repository.AddAsync(contract, cancellationToken);

        return MapToDto(contract);
    }

    public async Task<ContractResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contract = await repository.GetByIdAsync(id, cancellationToken);
        return contract is null ? null : MapToDto(contract);
    }

    public async Task<IReadOnlyList<ContractResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var contracts = await repository.GetAllAsync(cancellationToken);
        return contracts.Select(MapToDto).ToList();
    }

    private static ContractResponseDto MapToDto(Contract contract)
    {
        return new ContractResponseDto(
            contract.Id,
            contract.ClientId,
            contract.Title,
            contract.TotalValue,
            (int)contract.Status, // Convertendo o Enum para int para facilitar no JSON
            contract.StartDate);
    }
}