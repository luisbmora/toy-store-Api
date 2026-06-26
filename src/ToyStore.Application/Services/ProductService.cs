using ToyStore.Application.DTOs;
using ToyStore.Domain.Entities;
using ToyStore.Domain.Interfaces;

namespace ToyStore.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        return MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            AgeRestriction = createProductDto.AgeRestriction,
            Company = createProductDto.Company,
            Price = createProductDto.Price,
            ImageUrl = createProductDto.ImageUrl
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        return MapToDto(product);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        product.Name = updateProductDto.Name;
        product.Description = updateProductDto.Description;
        product.AgeRestriction = updateProductDto.AgeRestriction;
        product.Company = updateProductDto.Company;
        product.Price = updateProductDto.Price;
        product.ImageUrl = updateProductDto.ImageUrl;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return false;
        }

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<ProductDto?> UpdateImageAsync(int id, string imageUrl)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
        {
            return null;
        }

        product.ImageUrl = imageUrl;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return MapToDto(product);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            AgeRestriction = product.AgeRestriction,
            Company = product.Company,
            Price = product.Price,
            ImageUrl = product.ImageUrl
        };
    }
}