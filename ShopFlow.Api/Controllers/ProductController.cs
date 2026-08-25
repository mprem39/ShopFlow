using Microsoft.AspNetCore.Mvc;
using ShopFlow.Api.Contracts.Products;
using ShopFlow.Api.Models;

namespace ShopFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("73cc57fb-d9ad-4be9-a01f-1d7a48ab1c48"),
                Name = "Mechanical Keyboard",
                Price = 4999
            },
            new Product
            {
                Id = Guid.Parse("73cc57fb-d9ad-4be9-a01f-1d7a48ab1c49"),
                Name = "Wireless Mouse",
                Price = 2499
            }
        };

    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = _products
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Price))
            .ToArray();

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetProduct(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
       var response = new ProductResponse(
       product.Id,
       product.Name,
       product.Price);
        return Ok(response);
    }

    [HttpPost]
    public IActionResult CreateProduct(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };
        _products.Add(product);
        // For now, we're not persisting it.

        var response = new ProductResponse(
            product.Id,
            product.Name,
            product.Price);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            response);
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpdateProduct(Guid id,UpdateProductRequest updateProductRequest)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        _products.Remove(product);

        var updateProduct = new Product
        {
            Id = id,
            Name = updateProductRequest.Name,
            Price = updateProductRequest.Price
        };

        // For now, we're not persisting it.
        _products.Add(updateProduct);
       
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteProduct(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        _products.Remove(product);
        return NoContent();
    }

    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new InvalidOperationException("This is a test exception.");
    }
}