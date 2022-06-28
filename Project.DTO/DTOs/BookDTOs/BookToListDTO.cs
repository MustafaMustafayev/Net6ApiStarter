using Project.DTO.DTOs.AuthorDTOs;
using Project.DTO.DTOs.CategoryDTOs;

namespace Project.DTO.DTOs.BookDTOs;

public record BookToListDTO
{
    public int BookId { get; set; }

    public string Name { get; set; }

    public string PublishedAt { get; set; }

    public string ISBN { get; set; }

    public string CoverImage { get; set; }

    public CategoryToListDTO Category { get; set; }

    public List<AuthorToListDTO> Authors { get; set; }
}