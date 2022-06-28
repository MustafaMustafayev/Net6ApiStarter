using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.UserDTOs;

public record UserToAddDTO
{
    [Required] public string Username { get; set; }

    [Required] public string Password { get; set; }

    [Compare("Password")] public string PasswordConfirmation { get; set; }
}