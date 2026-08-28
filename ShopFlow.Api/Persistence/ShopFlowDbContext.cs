using Microsoft.EntityFrameworkCore;


namespace ShopFlow.Api.Persistence;

public sealed class ShopFlowDbContext : DbContext
{
    public ShopFlowDbContext(
        DbContextOptions<ShopFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ShopFlowDbContext).Assembly);
    }
}