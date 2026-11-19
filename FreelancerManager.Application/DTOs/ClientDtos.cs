namespace FreelancerManager.Application.DTOs;

public sealed record CreateClientRequestDto(string Name, string Email, string Document);
public sealed record ClientResponseDto(Guid Id, string Name, string Email, string Document);