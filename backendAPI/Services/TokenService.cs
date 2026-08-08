using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backendAPI.Entities;
using backendAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace backendAPI.Services;


public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot get token key");

        if (tokenKey.Length < 64) throw new Exception("Your token needs to be >= 64 charachters");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // pravime private key

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); //pravime objekt za potpisuvanje na tokenot

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // za development,
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler(); // ova koristime za kreiranje na jwt tokenot

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);                  // vrakame vo string
 

    }
}