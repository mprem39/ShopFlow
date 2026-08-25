using System.ComponentModel.DataAnnotations;

namespace ShopFlow.Api.Contracts.Products;

public sealed record UpdateProductRequest(
    [StringLength(20)]
    string Name,
    [Range(0,100000)]
    decimal Price
    );