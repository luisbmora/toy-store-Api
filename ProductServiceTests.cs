using FluentAssertions;
using Moq;
using ToyStore.Application.DTOs;
using ToyStore.Application.Services;
using ToyStore.Domain.Entities;
using ToyStore.Domain.Interfaces;

namespace ToyStore.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Carro Hot Wheels",
                Company = "Mattel",
                Price = 89.99m
            },
            new Product
            {
                Id = 2,
                Name = "Set LEGO City",
                Company = "LEGO",
                Price = 799.00m
            }
        };

        _productRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Carro Hot Wheels");
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ShouldReturnProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Carro Hot Wheels",
            Company = "Mattel",
            Price = 89.99m
        };

        _productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("Carro Hot Wheels");
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(99))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _productService.GetByIdAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateProduct()
    {
        // Arrange
        var createProductDto = new CreateProductDto
        {
            Name = "Oso de Peluche",
            Description = "Peluche suave para niños.",
            AgeRestriction = 2,
            Company = "ToyLand",
            Price = 249.99m,
            ImageUrl = null
        };

        _productRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(product => product.Id = 10)
            .Returns(Task.CompletedTask);

        _productRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _productService.CreateAsync(createProductDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Name.Should().Be("Oso de Peluche");
        result.Company.Should().Be("ToyLand");

        _productRepositoryMock.Verify(
            repository => repository.AddAsync(It.IsAny<Product>()),
            Times.Once);

        _productRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_ShouldReturnTrue()
    {
        // Arrange
        var existingProduct = new Product
        {
            Id = 1,
            Name = "Carro Hot Wheels",
            Company = "Mattel",
            Price = 89.99m
        };

        var updateProductDto = new UpdateProductDto
        {
            Name = "Carro Hot Wheels Edición Especial",
            Description = "Carro actualizado.",
            AgeRestriction = 3,
            Company = "Mattel",
            Price = 99.99m,
            ImageUrl = null
        };

        _productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);

        _productRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _productService.UpdateAsync(1, updateProductDto);

        // Assert
        result.Should().BeTrue();
        existingProduct.Name.Should().Be("Carro Hot Wheels Edición Especial");
        existingProduct.Price.Should().Be(99.99m);

        _productRepositoryMock.Verify(
            repository => repository.Update(existingProduct),
            Times.Once);

        _productRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_ShouldReturnTrue()
    {
        // Arrange
        var existingProduct = new Product
        {
            Id = 1,
            Name = "Carro Hot Wheels",
            Company = "Mattel",
            Price = 89.99m
        };

        _productRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);

        _productRepositoryMock
            .Setup(repository => repository.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _productService.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();

        _productRepositoryMock.Verify(
            repository => repository.Delete(existingProduct),
            Times.Once);

        _productRepositoryMock.Verify(
            repository => repository.SaveChangesAsync(),
            Times.Once);
    }
}