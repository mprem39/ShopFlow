namespace ShopFlow.Api.Contracts.Products;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price);