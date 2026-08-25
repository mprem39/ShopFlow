using System.ComponentModel.DataAnnotations;

namespace ShopFlow.Api.Contracts.Products;

public sealed record CreateProductRequest(
    [StringLength(20)]
    string Name,
    [Range(0.01,100000)]
    decimal Price
    );