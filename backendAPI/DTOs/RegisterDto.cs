using System.ComponentModel.DataAnnotations;

namespace backendAPI.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; } = ""; // so prazno mesto e required

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";
   
   [Required]
   [MinLength(6)]
    public string Password { get; set; } = "";
}