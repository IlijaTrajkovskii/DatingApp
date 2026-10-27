using System.ComponentModel.DataAnnotations;

namespace backendAPI.DTOs;

public class RegisterDto
{
    [Required] 
    public string DisplayName { get; set; } = ""; // so prazno mesto e required

    [Required]
    [EmailAddress]  //validacija na podatoci sto stigaat vo api-to
    public string Email { get; set; } = "";
   
   [Required]
   [MinLength(6)]
    public string Password { get; set; } = "";

    [Required]
    public string Gender { get; set; } = "";

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public string City { get; set; } = ""; 

    [Required]
    public string Country { get; set; } = "";
}