using System.ComponentModel.DataAnnotations.Schema;
using backendAPI.Entities;

namespace backendAPI.DTOs;


public class UserSeedDto
{
    public required string Id {get; set;}

    public required string Email {get; set;}

    public DateOnly DateOfBirth {get; set;} 

    public string? ImageUrl {get; set;}
    
    public required string DisplayName {get; set;} 

    public DateTime Created {get; set;} 

    public DateTime LastActive {get; set;}

    public required string Gender {get; set;}

    public string? Description {get; set;}

    public required string City {get; set;}

    public required string Country {get; set;}

    
    [ForeignKey(nameof(Id))]
    public AppUser User {get; set;} = null!;
}