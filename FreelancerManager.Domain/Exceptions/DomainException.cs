namespace FreelancerManager.Domain.Exceptions;

/// <summary>
/// Exceção lançada quando uma invariante de negócio é violada.
/// Deve ser capturada por um middleware global na camada API.
/// </summary>
public sealed class DomainException(string message) : Exception(message);