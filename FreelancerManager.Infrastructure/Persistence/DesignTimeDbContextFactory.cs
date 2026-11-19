using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FreelancerManager.Infrastructure.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FreelancerManagerDbContext>
{
    public FreelancerManagerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FreelancerManagerDbContext>();

        // Mesma string de conexão usada em runtime (ou uma específica para design-time).
        // Como o objetivo aqui é apenas gerar a migration, o SQLite local é suficiente.
        optionsBuilder.UseSqlite("Data Source=freelancer_design.db");

        return new FreelancerManagerDbContext(optionsBuilder.Options);
    }
}