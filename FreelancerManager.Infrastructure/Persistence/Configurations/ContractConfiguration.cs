using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelancerManager.Infrastructure.Persistence.Configurations;

public sealed class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
	public void Configure(EntityTypeBuilder<Contract> builder)
	{
		builder.ToTable("Contracts");
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Id).ValueGeneratedNever();

		builder.Property(c => c.ClientId).IsRequired();
		builder.Property(c => c.Title).IsRequired().HasMaxLength(150);
		builder.Property(c => c.TotalValue).IsRequired().HasPrecision(18, 2);
		builder.Property(c => c.Status).IsRequired();
		builder.Property(c => c.StartDate).IsRequired();

		// Relacionamento: 1 Cliente tem N Contratos
		builder.HasOne<Client>()
			   .WithMany()
			   .HasForeignKey(c => c.ClientId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}