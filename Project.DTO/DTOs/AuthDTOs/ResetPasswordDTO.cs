using System.ComponentModel.DataAnnotations;

namespace Project.DTO.DTOs.AuthDTOs;

public record ResetPasswordDTO
{
    [Required] public int UserId { get; set; }

    [Required] public string Password { get; set; }

    [Required] [Compare("Password")] public string PasswordConfirmation { get; set; }
}