using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.AuthDTOs;

public record LoginDTO
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }
}