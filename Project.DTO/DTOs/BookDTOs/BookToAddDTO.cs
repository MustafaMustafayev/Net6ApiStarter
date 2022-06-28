using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.BookDTOs;

public record BookToAddDTO
{
    [Required] public string Name { get; set; }

    [Required] public DateTime PublishedAt { get; set; }

    [Required] public string ISBN { get; set; }

    public string CoverImage { get; set; } // value will come from response of file uploading process

    [Required] public int CategoryId { get; set; }

    [Required] public List<int> AuthorIds { get; set; }
}