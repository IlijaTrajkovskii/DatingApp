
using backendAPI.Entities;

namespace backendAPI.Interfaces;


public interface ITokenService
{
    string CreateToken(AppUser user);
}