using FreelancerManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreelancerManager.Infrastructure.Persistence;

public sealed class FreelancerManagerDbContext(DbContextOptions<FreelancerManagerDbContext> options)
    : DbContext(options)
{
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contract> Contracts => Set<Contract>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica TODAS as IEntityTypeConfiguration<> do assembly de uma vez.
        // Novas entidades → só criar sua Configuration, nada aqui precisa mudar.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FreelancerManagerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}