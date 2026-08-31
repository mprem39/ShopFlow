using FluentAssertions;
using NSubstitute;
using ShopFlow.Api.Contracts.Products;
using ShopFlow.Api.Persistence.Repositories;
using ShopFlow.Api.Services;
using ShopFlow.Api.Services.Products;
using System.Collections;

namespace ShopFlow.UnitTests.Services;

public sealed class ProductServiceTests
{
    private readonly ProductService _sut;
    private readonly IProductRepository _productRepository= Substitute.For<IProductRepository>();

    public ProductServiceTests()
    {
        _sut = new ProductService(_productRepository);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenNoProductExists()
    {
        //Arrange
        _productRepository.GetProductsAsync().Returns(Array.Empty<Product>());
        var service = new ProductService(_productRepository);

        // Act
        var result = await service.GetProductsAsync();

        //Assert
        result.Should().BeEmpty();

        await _productRepository.Received(1).GetProductsAsync();

    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnProducts_WhenSomeProductsExist()
    {
        // Arrange
        var products = new List<Product>
    {
        new Product("Mechanical Keyboard", 4999m),
        new Product("Wireless Mouse", 2499m)
    };


        _productRepository
            .GetProductsAsync()
            .Returns(products);

        var service = new ProductService(_productRepository);

        // Act
        var result = await service.GetProductsAsync();

        // Assert
        result.Should().BeEquivalentTo(
            products.Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.Price)));

        await _productRepository
            .Received(1)
            .GetProductsAsync();
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product(
            "Mechanical Keyboard",
            4999m);

        _productRepository
            .GetByIdAsync(product.Id)
            .Returns(product);

        // Act
        var result = await _sut.GetProductAsync(product.Id);

        // Assert
        result.Should().NotBeNull();

        result.Should().BeEquivalentTo(
            new ProductResponse(
                product.Id,
                product.Name,
                product.Price));

        await _productRepository
            .Received(1)
            .GetByIdAsync(product.Id);
    }

    [Fact]
    public async Task GetProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepository
            .GetByIdAsync(productId)
            .Returns((Product?)null);

        // Act
        var result = await _sut.GetProductAsync(productId);

        // Assert
        result.Should().BeNull();

        await _productRepository
            .Received(1)
            .GetByIdAsync(productId);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldCreateProduct_WhenRequestIsValid()
    {
        // Arrange
        var request = new CreateProductRequest(
            "Gaming Keyboard",
            5999m);

        // Act
        var result = await _sut.CreateProductAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Name.Should().Be(request.Name);
        result.Price.Should().Be(request.Price);

        await _productRepository
            .Received(1)
            .AddAsync(Arg.Is<Product>(product =>
                product.Name == request.Name &&
                product.Price == request.Price));

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var request = new UpdateProductRequest(
            "Gaming Keyboard",
            5999m);

        _productRepository
            .GetByIdAsync(productId)
            .Returns((Product?)null);

        // Act
        var result = await _sut.UpdateProductAsync(
            productId,
            request);

        // Assert
        result.Should().BeFalse();

        await _productRepository
            .Received(1)
            .GetByIdAsync(productId);

        await _productRepository
            .DidNotReceive()
            .SaveChangesAsync();
    }
    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product(
            "Mechanical Keyboard",
            4999m);

        var request = new UpdateProductRequest(
            "Gaming Keyboard",
            5999m);

        _productRepository
            .GetByIdAsync(product.Id)
            .Returns(product);

        // Act
        var result = await _sut.UpdateProductAsync(
            product.Id,
            request);

        // Assert
        result.Should().BeTrue();

        product.Name.Should().Be(request.Name);
        product.Price.Should().Be(request.Price);

        await _productRepository
            .Received(1)
            .GetByIdAsync(product.Id);

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }
    [Fact]
    public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepository
            .GetByIdAsync(productId)
            .Returns((Product?)null);

        // Act
        var result = await _sut.DeleteProductAsync(productId);

        // Assert
        result.Should().BeFalse();

        await _productRepository
            .Received(1)
            .GetByIdAsync(productId);

        _productRepository
            .DidNotReceive()
            .Remove(Arg.Any<Product>());

        await _productRepository
            .DidNotReceive()
            .SaveChangesAsync();
    }
    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product(
            "Mechanical Keyboard",
            4999m);

        _productRepository
            .GetByIdAsync(product.Id)
            .Returns(product);

        // Act
        var result = await _sut.DeleteProductAsync(product.Id);

        // Assert
        result.Should().BeTrue();

        await _productRepository
            .Received(1)
            .GetByIdAsync(product.Id);

        _productRepository
            .Received(1)
            .Remove(product);

        await _productRepository
            .Received(1)
            .SaveChangesAsync();
    }
}