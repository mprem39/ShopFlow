using Microsoft.EntityFrameworkCore;

namespace ShopFlow.Api.Persistence.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly ShopFlowDbContext _dbContext;

    public ProductRepository(ShopFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        await _dbContext.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        _dbContext.Products.Remove(product);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}