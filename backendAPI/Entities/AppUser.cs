namespace backendAPI.Entities;


public class AppUser
{
    
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public required string DisplayName { get; set; }

    public required string Email { get; set; }   // public string? Email bi ostavilo da e nullable no treba proverka podocna
                              
    public string? ImageUrl {get; set;}                                                
    public required byte[] PasswordHash { get; set; }

    public required byte[] PasswordSalt { get; set; }



    // 1:1 rltshp
    public Member Member {get; set; } = null!; // FK kon Member
                                                
}

