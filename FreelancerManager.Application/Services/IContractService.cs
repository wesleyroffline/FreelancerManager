using FreelancerManager.Application.DTOs;

namespace FreelancerManager.Application.Services;

public interface IContractService
{
    Task<ContractResponseDto> CreateAsync(CreateContractRequestDto request, CancellationToken cancellationToken = default);
    Task<ContractResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ContractResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
}