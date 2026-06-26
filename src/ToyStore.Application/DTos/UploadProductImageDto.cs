using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ToyStore.Application.DTOs;

public class UploadProductImageDto
{
    [Required(ErrorMessage = "La imagen del producto es obligatoria.")]
    public IFormFile Image { get; set; } = null!;
}