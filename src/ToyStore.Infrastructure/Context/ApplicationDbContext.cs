using Microsoft.EntityFrameworkCore;
using ToyStore.Domain.Entities;

namespace ToyStore.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);

            entity.Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(product => product.Description)
                .HasMaxLength(100);

            entity.Property(product => product.Company)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(product => product.Price)
                .IsRequired();

            entity.Property(product => product.ImageUrl)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Carro Hot Wheels",
                Description = "Carro metálico de colección.",
                AgeRestriction = 3,
                Company = "Mattel",
                Price = 89.99m,
                ImageUrl = null
            },
            new Product
            {
                Id = 2,
                Name = "Set LEGO City",
                Description = "Set de construcción para niños.",
                AgeRestriction = 6,
                Company = "LEGO",
                Price = 799.00m,
                ImageUrl = null
            },
            new Product
            {
                Id = 3,
                Name = "Muñeca Barbie",
                Description = "Muñeca con accesorios incluidos.",
                AgeRestriction = 3,
                Company = "Mattel",
                Price = 349.50m,
                ImageUrl = null
            },
            new Product
            {
                Id = 4,
                Name = "Dinosaurio Interactivo",
                Description = "Juguete con luces y sonidos.",
                AgeRestriction = 5,
                Company = "Hasbro",
                Price = 599.99m,
                ImageUrl = null
            },
            new Product
            {
                Id = 5,
                Name = "Rompecabezas Infantil",
                Description = "Rompecabezas educativo de 100 piezas.",
                AgeRestriction = 4,
                Company = "Ravensburger",
                Price = 199.00m,
                ImageUrl = null
            }
        );
    }
}