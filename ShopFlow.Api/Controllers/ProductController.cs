using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopFlow.Api.Contracts.Products;
using ShopFlow.Api.Data;

namespace ShopFlow.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ShopFlowDbContext _dbContext;
    public ProductsController(ShopFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {

        var products = await _dbContext.Products
            .AsNoTracking()
            .Select(product => new ProductResponse(
                product.Id,
                product.Name,
                product.Price))
            .ToListAsync();

        return Ok(products);
    }
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Price))
            .FirstOrDefaultAsync();

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        var product = new Product(
        request.Name,
        request.Price);
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
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
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        UpdateProductRequest request)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        product.Update(
            request.Name,
            request.Price);

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }


}