using FreelancerManager.Application.DTOs;

namespace FreelancerManager.Application.Services;

public interface IClientService
{
	Task<ClientResponseDto> CreateAsync(CreateClientRequestDto request, CancellationToken cancellationToken = default);
	Task<ClientResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<ClientResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}