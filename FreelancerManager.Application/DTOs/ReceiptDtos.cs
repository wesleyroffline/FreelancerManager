namespace FreelancerManager.Application.DTOs;

// O que a API recebe
public sealed record CreateReceiptRequestDto(
    Guid ContractId,
    string ReceiptNumber,
    decimal GrossValue,
    decimal CustomTaxPercentage, // Ex: 5.5 para 5.5% de imposto
    string PaymentMethod);

// O que a API devolve
public sealed record ReceiptResponseDto(
    Guid Id,
    Guid ContractId,
    string ReceiptNumber,
    decimal GrossValue,
    decimal TaxDeductions,
    decimal NetValue,
    string PaymentMethod,
    DateTime IssuedAt);