using ShopFlow.Api.Contracts.Products;

using ShopFlow.Api.Persistence.Repositories;

namespace ShopFlow.Api.Services.Products;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<ProductResponse>> GetProductsAsync()
    {
        var products = await _productRepository.GetProductsAsync();

        return products
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Price))
            .ToList();
    }

    public async Task<ProductResponse?> GetProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price);
    }

    public async Task<ProductResponse> CreateProductAsync(
        CreateProductRequest request)
    {
        var product = new Product(
            request.Name,
            request.Price);

        await _productRepository.AddAsync(product);

        await _productRepository.SaveChangesAsync();

        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price);
    }

    public async Task<bool> UpdateProductAsync(
        Guid id,
        UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        product.Update(
            request.Name,
            request.Price);

        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        _productRepository.Remove(product);

        await _productRepository.SaveChangesAsync();

        return true;
    }
}