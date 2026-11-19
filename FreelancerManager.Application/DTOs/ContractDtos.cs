namespace FreelancerManager.Application.DTOs;

public sealed record CreateContractRequestDto(Guid ClientId, string Title, decimal TotalValue, DateTime StartDate);
public sealed record ContractResponseDto(Guid Id, Guid ClientId, string Title, decimal TotalValue, int Status, DateTime StartDate);