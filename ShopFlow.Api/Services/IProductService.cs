using ShopFlow.Api.Contracts.Products;

namespace ShopFlow.Api.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> GetProductsAsync();

    Task<ProductResponse?> GetProductAsync(Guid id);

    Task<ProductResponse> CreateProductAsync(
        CreateProductRequest request);

    Task<bool> UpdateProductAsync(
        Guid id,
        UpdateProductRequest request);

    Task<bool> DeleteProductAsync(Guid id);
}
