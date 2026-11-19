using FreelancerManager.Domain.Exceptions;

namespace FreelancerManager.Domain.Entities;

/// <summary>
/// Raiz de agregado que representa um recibo emitido por um freelancer.
/// Auto-validável via Guard Clauses; não depende de bibliotecas externas.
/// </summary>
public sealed class Receipt
{
    public Guid Id { get; private set; }
    public Guid ContractId { get; private set; }
    public Contract Contract { get; private set; } = null!;
    public string ReceiptNumber { get; private set; } = null!;
    public decimal GrossValue { get; private set; }
    public decimal TaxDeductions { get; private set; }
    public decimal NetValue { get; private set; }
    public string PaymentMethod { get; private set; } = null!;
    public DateTime IssuedAt { get; private set; }

    // Construtor privado exigido pelo EF Core para materialização.
    private Receipt() { }

    public Receipt(
        Guid contractId,
        string receiptNumber,
        decimal grossValue,
        decimal taxDeductions,
        string paymentMethod,
        DateTime issuedAt)
    {
        ValidateContractId(contractId);
        ValidateReceiptNumber(receiptNumber);
        ValidateGrossValue(grossValue);
        ValidateTaxDeductions(taxDeductions, grossValue);
        ValidatePaymentMethod(paymentMethod);

        Id = Guid.NewGuid();
        ContractId = contractId;
        ReceiptNumber = receiptNumber.Trim();
        GrossValue = grossValue;
        TaxDeductions = taxDeductions;
        NetValue = CalculateNetValue(grossValue, taxDeductions);
        PaymentMethod = paymentMethod.Trim();
        IssuedAt = issuedAt;
    }

    /// <summary>
    /// Ajuste financeiro de um recibo já emitido (estorno, correção de nota).
    /// ReceiptNumber, ContractId e IssuedAt permanecem imutáveis por regra fiscal.
    /// </summary>
    public void Update(decimal grossValue, decimal taxDeductions, string paymentMethod)
    {
        ValidateGrossValue(grossValue);
        ValidateTaxDeductions(taxDeductions, grossValue);
        ValidatePaymentMethod(paymentMethod);

        GrossValue = grossValue;
        TaxDeductions = taxDeductions;
        NetValue = CalculateNetValue(grossValue, taxDeductions);
        PaymentMethod = paymentMethod.Trim();
    }

    // ----------------------------------------------------------------
    // REGRA DE NEGÓCIO CRUCIAL:
    // NetValue = GrossValue - TaxDeductions.
    // NÃO há cálculo automático de INSS/IRRF/ISS aqui. O domínio
    // desconhece tributos específicos — a camada de Application é
    // responsável por compor o total de deduções personalizadas.
    // ----------------------------------------------------------------
    private static decimal CalculateNetValue(decimal grossValue, decimal taxDeductions)
        => grossValue - taxDeductions;

    // -------------------- Guard Clauses --------------------
    private static void ValidateContractId(Guid contractId)
    {
        if (contractId == Guid.Empty)
            throw new DomainException("ContractId deve ser um GUID válido e não vazio.");
    }

    private static void ValidateReceiptNumber(string receiptNumber)
    {
        if (string.IsNullOrWhiteSpace(receiptNumber))
            throw new DomainException("O número do recibo é obrigatório.");
    }

    private static void ValidateGrossValue(decimal grossValue)
    {
        if (grossValue <= 0)
            throw new DomainException("O valor bruto (GrossValue) deve ser maior que zero.");
    }

    private static void ValidateTaxDeductions(decimal taxDeductions, decimal grossValue)
    {
        if (taxDeductions < 0)
            throw new DomainException("As deduções (TaxDeductions) não podem ser negativas.");

        if (taxDeductions > grossValue)
            throw new DomainException("As deduções não podem ser maiores que o valor bruto.");
    }

    private static void ValidatePaymentMethod(string paymentMethod)
    {
        if (string.IsNullOrWhiteSpace(paymentMethod))
            throw new DomainException("A forma de pagamento é obrigatória.");
    }
}