using Microsoft.AspNetCore.Http;

namespace ToyStore.Application.Services;

public class FileStorageService
{
    private readonly string[] _allowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public async Task<string> SaveProductImageAsync(
        IFormFile image,
        string webRootPath)
    {
        if (image.Length <= 0)
        {
            throw new ArgumentException("La imagen no puede estar vacía.");
        }

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(extension))
        {
            throw new ArgumentException("Solo se permiten imágenes JPG, JPEG, PNG o WEBP.");
        }

        var folderPath = Path.Combine(
            webRootPath,
            "images",
            "products");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);

        await image.CopyToAsync(stream);

        return $"/images/products/{fileName}";
    }
}