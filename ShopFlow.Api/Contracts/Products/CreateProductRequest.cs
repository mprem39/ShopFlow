namespace ShopFlow.Api.Contracts.Products;

public sealed record CreateProductRequest(
    string Name,
    decimal Price);