using System.ComponentModel.DataAnnotations;

namespace ToyStore.Application.DTOs;

public class CreateProductDto
{
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    [MaxLength(50, ErrorMessage = "El nombre del producto no puede exceder los 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
    public string? Description { get; set; }

    [Range(0, 100, ErrorMessage = "La restricción de edad debe estar entre 0 y 100.")]
    public int? AgeRestriction { get; set; }

    [Required(ErrorMessage = "La compañía es obligatoria.")]
    [MaxLength(50, ErrorMessage = "La compañía no puede exceder los 50 caracteres.")]
    public string Company { get; set; } = string.Empty;

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(1, 1000, ErrorMessage = "El precio debe estar entre 1 y 1000.")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }
}