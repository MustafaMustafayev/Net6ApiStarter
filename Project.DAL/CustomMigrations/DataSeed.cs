using Microsoft.EntityFrameworkCore;
using Project.Entity.Entities;

namespace Project.DAL.CustomMigrations;

public static class DataSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Crime" },
            new Category { CategoryId = 2, Name = "Fantasy" },
            new Category { CategoryId = 3, Name = "Philosophical" });
    }
}