namespace backendAPI.Entities;


public class AppUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string displayName { get; set; }

    public required string Email { get; set; }   // public string? Email bi ostavilo da e nullable no treba proverka podocna
                                                

}

