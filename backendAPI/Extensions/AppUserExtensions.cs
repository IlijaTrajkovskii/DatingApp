

using backendAPI.DTOs;
using backendAPI.Entities;
using backendAPI.Interfaces;

namespace backendAPI.Extensions;


// koga e static funckiite stanuvaat globalno dostapni bez potreba od instanca od klasata
// so static classes nemozes dependecy injection pa go preprakame kako paramater vo funkcija
//  a ne vo konstruktor
public static class AppUserExtensions
{
 
    public static UserDtoResponse toDto(this AppUser user, ITokenService tokenService)
    {
        return new UserDtoResponse
        {
            Id = user.Id,
            DisplayName = user.DisplayName,
            Email = user.Email,
            ImageUrl = user.ImageUrl,
            Token = tokenService.CreateToken(user)
        };
    }


}