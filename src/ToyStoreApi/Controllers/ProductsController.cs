using Microsoft.AspNetCore.Mvc;
using ToyStore.Application.DTOs;
using ToyStore.Application.Services;

namespace ToyStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Obtiene todos los productos disponibles.
    /// </summary>
    /// <returns>Lista de productos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();

        return Ok(products);
    }

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return Ok(product);
    }

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    /// <param name="createProductDto">Datos del producto a crear.</param>
    /// <returns>Producto creado.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> Create(CreateProductDto createProductDto)
    {
        var createdProduct = await _productService.CreateAsync(createProductDto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct);
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="updateProductDto">Datos actualizados del producto.</param>
    /// <returns>Resultado de la actualización.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateProductDto updateProductDto)
    {
        var updated = await _productService.UpdateAsync(id, updateProductDto);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return NoContent();
    }

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Resultado de la eliminación.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return NoContent();
    }
}