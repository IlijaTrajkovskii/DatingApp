using System.Text.Json.Serialization;

namespace backendAPI.Entities;


public class Photo
{
    public int Id {get; set;}
    public required string Url {get; set;}
    public string? PublicId {get; set;} //  storage

    // 1:m relationship foreign key odi od stranata na m (many)
    // a member can have many photos 1:m
    [JsonIgnore]
    public Member Member {get; set;} = null!; // FK
    public string MemberId {get; set;} = null!;


}