using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.UserDTOs;

public record UserToUpdateDTO
{
    [Required] public int UserId { get; set; }

    [Required] public string Username { get; set; }
}