namespace Project.DTO.DTOs.CategoryDTOs;

public record CategoryToListDTO
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public CategoryToListDTO ParentCategory { get; set; }
}