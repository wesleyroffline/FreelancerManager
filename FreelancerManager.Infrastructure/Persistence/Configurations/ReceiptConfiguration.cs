using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelancerManager.Infrastructure.Persistence.Configurations;

public sealed class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.ToTable("Receipts");

        // -------- PK --------
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        // -------- Campos --------
        builder.Property(r => r.ContractId)
            .IsRequired();

        builder.Property(r => r.ReceiptNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.GrossValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.TaxDeductions)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.NetValue)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.IssuedAt)
            .IsRequired();

        // -------- Índices --------
        builder.HasIndex(r => r.ContractId)
            .HasDatabaseName("IX_Receipts_ContractId");

        builder.HasIndex(r => r.ReceiptNumber)
            .IsUnique()
            .HasDatabaseName("IX_Receipts_ReceiptNumber_Unique");

        // Relacionamento comentado — habilitar quando a entidade Contract existir.
        // builder.HasOne<Contract>()
        //     .WithMany()
        //     .HasForeignKey(r => r.ContractId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}