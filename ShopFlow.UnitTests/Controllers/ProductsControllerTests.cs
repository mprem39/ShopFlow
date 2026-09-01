using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ShopFlow.Api.Contracts.Products;
using ShopFlow.Api.Controllers;
using ShopFlow.Api.Services;

namespace ShopFlow.UnitTests.Controllers;

public sealed class ProductsControllerTests
{
    private readonly IProductService _productService;
    private readonly ProductsController _sut;

    public ProductsControllerTests()
    {
        _productService = Substitute.For<IProductService>();
        _sut = new ProductsController(_productService);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnOk_WhenProductsExist()
    {
        // Arrange
        var products = new List<ProductResponse>
    {
        new(
            Guid.NewGuid(),
            "Mechanical Keyboard",
            4999m),

        new(
            Guid.NewGuid(),
            "Wireless Mouse",
            2499m)
    };

        _productService
            .GetProductsAsync()
            .Returns(products);

        // Act
        var result = await _sut.GetProducts();

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;

        okResult.Value.Should().BeEquivalentTo(products);

        await _productService
            .Received(1)
            .GetProductsAsync();
    }
    [Fact]
    public async Task GetProduct_ShouldReturnOk_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var product = new ProductResponse(
            productId,
            "Mechanical Keyboard",
            4999m);

        _productService
            .GetProductAsync(productId)
            .Returns(product);

        // Act
        var result = await _sut.GetProduct(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var okResult = (OkObjectResult)result;

        okResult.Value.Should().BeEquivalentTo(product);

        await _productService
            .Received(1)
            .GetProductAsync(productId);
    }
    [Fact]
    public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productService
            .GetProductAsync(productId)
            .Returns((ProductResponse?)null);

        // Act
        var result = await _sut.GetProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();

        await _productService
            .Received(1)
            .GetProductAsync(productId);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnCreated_WhenProductIsCreated()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Gaming Keyboard",
            5999m);

        var productId = Guid.NewGuid();

        var product = new ProductResponse(
            productId,
            request.Name,
            request.Price);

        _productService
            .CreateProductAsync(request)
            .Returns(product);

        // Act
        var result = await _sut.CreateProduct(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;

        createdResult.ActionName.Should().Be(nameof(ProductsController.GetProduct));

        createdResult.RouteValues.Should().ContainKey("id");
        createdResult.RouteValues!["id"].Should().Be(productId);

        createdResult.Value.Should().BeEquivalentTo(product);

        await _productService
            .Received(1)
            .CreateProductAsync(request);
    }
    [Fact]
    public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var request = new UpdateProductRequest(
            "Gaming Keyboard",
            5999m);

        _productService
            .UpdateProductAsync(productId, request)
            .Returns(false);

        // Act
        var result = await _sut.UpdateProduct(
            productId,
            request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();

        await _productService
            .Received(1)
            .UpdateProductAsync(productId, request);
    }
    [Fact]
    public async Task UpdateProduct_ShouldReturnNoContent_WhenProductIsUpdated()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var request = new UpdateProductRequest(
            "Gaming Keyboard",
            5999m);

        _productService
            .UpdateProductAsync(productId, request)
            .Returns(true);

        // Act
        var result = await _sut.UpdateProduct(
            productId,
            request);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        await _productService
            .Received(1)
            .UpdateProductAsync(productId, request);
    }
    [Fact]
    public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productService
            .DeleteProductAsync(productId)
            .Returns(false);

        // Act
        var result = await _sut.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();

        await _productService
            .Received(1)
            .DeleteProductAsync(productId);
    }
    [Fact]
    public async Task DeleteProduct_ShouldReturnNoContent_WhenProductIsDeleted()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productService
            .DeleteProductAsync(productId)
            .Returns(true);

        // Act
        var result = await _sut.DeleteProduct(productId);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        await _productService
            .Received(1)
            .DeleteProductAsync(productId);
    }
}