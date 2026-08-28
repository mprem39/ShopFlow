namespace ShopFlow.Api.Persistence.Repositories;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetProductsAsync();

    Task<Product?> GetByIdAsync(Guid id);

    Task AddAsync(Product product);

    void Remove(Product product);

    Task SaveChangesAsync();
}
