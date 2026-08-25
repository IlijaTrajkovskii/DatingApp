using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace backendAPI.Entities;


public class Member
{
    
    public string Id {get; set;} = null!;

    public DateOnly DateOfBirth {get; set;} 

    public string? ImageUrl {get; set;}
    
    public required string DisplayName {get; set;} 

    public DateTime Created {get; set;} = DateTime.UtcNow;

    public DateTime LastActive {get; set;} = DateTime.UtcNow;

    public required string Gender {get; set;}

    public string? Description {get; set;}

    public required string City {get; set;}

    public required string Country {get; set;}

    [JsonIgnore]
    public List<Photo> Photos {get; set;} = [];


    // navigation property / Foreign KEY
    // pr. Koga ke go imame dadeniot member mozeme preku foreign key-to (User) 
    // da pristapime do atributite na userot - RELEATIONSHIPS 

    [JsonIgnore]
    [ForeignKey(nameof(Id))]
    public AppUser User {get; set;} = null!;




}