using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.AuthorDTOs;

public record AuthorToAddDTO
{
    [Required] public string Name { get; set; }

    [Required] public string Surname { get; set; }
}