

namespace backendAPI.DTOs;

public class LoginDto()
{
    public string email { get; set; } = "";
    public string password { get; set; } = "";

    //ne stavame tuka validacija zosto pravicme vo servis
}