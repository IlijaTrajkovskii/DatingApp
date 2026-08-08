

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
            DisplayName = user.displayName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }


}