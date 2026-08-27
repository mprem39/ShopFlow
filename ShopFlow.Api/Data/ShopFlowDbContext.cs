using Microsoft.EntityFrameworkCore;


namespace ShopFlow.Api.Data;

public sealed class ShopFlowDbContext : DbContext
{
    public ShopFlowDbContext(DbContextOptions<ShopFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}