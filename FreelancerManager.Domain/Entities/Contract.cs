using FreelancerManager.Domain.Enums;
using FreelancerManager.Domain.Exceptions;

namespace FreelancerManager.Domain.Entities;

public sealed class Contract
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public string Title { get; private set; } = null!;
    public decimal TotalValue { get; private set; }
    public ContractStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }

    private Contract() { }

    public Contract(Guid clientId, string title, decimal totalValue, DateTime startDate)
    {
        ValidateClientId(clientId);
        ValidateTitle(title);
        ValidateTotalValue(totalValue);

        Id = Guid.NewGuid();
        ClientId = clientId;
        Title = title.Trim();
        TotalValue = totalValue;
        StartDate = startDate;
        Status = ContractStatus.Active; // Começa como Ativo por padrão
    }

    public void CompleteContract()
    {
        if (Status != ContractStatus.Active)
            throw new DomainException("Apenas contratos ativos podem ser concluídos.");

        Status = ContractStatus.Completed;
    }

    public void CancelContract()
    {
        if (Status != ContractStatus.Active)
            throw new DomainException("Apenas contratos ativos podem ser cancelados.");

        Status = ContractStatus.Cancelled;
    }

    private static void ValidateClientId(Guid clientId)
    {
        if (clientId == Guid.Empty)
            throw new DomainException("O ID do cliente é obrigatório.");
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("O título do contrato é obrigatório.");
    }

    private static void ValidateTotalValue(decimal totalValue)
    {
        if (totalValue <= 0)
            throw new DomainException("O valor total do contrato deve ser maior que zero.");
    }
}