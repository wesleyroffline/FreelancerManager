using FreelancerManager.Domain.Exceptions;

namespace FreelancerManager.Domain.Entities;

public sealed class Client
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Document { get; private set; } = null!; // CPF ou CNPJ
    public DateTime CreatedAt { get; private set; }

    private Client() { }

    public Client(string name, string email, string document)
    {
        ValidateName(name);
        ValidateEmail(email);
        ValidateDocument(document);

        Id = Guid.NewGuid();
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        Document = document.Trim();
        CreatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome do cliente é obrigatório.");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("O e-mail informado é inválido.");
    }

    private static void ValidateDocument(string document)
    {
        if (string.IsNullOrWhiteSpace(document) || document.Length < 11)
            throw new DomainException("O documento (CPF/CNPJ) é inválido.");
    }
}