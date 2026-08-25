using Microsoft.AspNetCore.Mvc;

namespace ShopFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly Product[] products = new[]
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Mechanical Keyboard",
                Price = 4999
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Wireless Mouse",
                Price = 2499
            }
        };

    [HttpGet]
    public IActionResult GetProducts()
    {
        
        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    private sealed class Product
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int Price { get; init; }
    }
}