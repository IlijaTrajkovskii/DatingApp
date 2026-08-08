

namespace backendAPI.DTOs;

public class LoginDto()
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    //ne stavame tuka validacija zosto pravicme vo servis
}