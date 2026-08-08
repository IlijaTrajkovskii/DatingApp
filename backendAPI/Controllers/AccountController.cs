
using System.Security.Cryptography;
using System.Text;
using backendAPI.Controllers;
using backendAPI.Data;
using backendAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using backendAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using backendAPI.Interfaces;
using backendAPI.Extensions;




public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
{
    
    [HttpPost("register")]  // odgovara na api/account/register
    public async Task<ActionResult<UserDtoResponse>> Register(RegisterDto registerDto)
    {
        if(await EmailExist(registerDto.Email)) return BadRequest("Email is already taken!");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            displayName = registerDto.DisplayName,
            Email = registerDto.Email,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.toDto(tokenService);
    }

    [HttpPost("login")]  // api/account/login
    public async Task<ActionResult<UserDtoResponse>> Login(LoginDto loginDto)
    {
        var user = await context.Users.SingleOrDefaultAsync(x => x.Email == loginDto.Email); // moze da bide i null pa pravime proverka

        if(user == null)
        {
            return Unauthorized("Invalid e-mail address !");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt); // go koristime istiot hash algoritam so kluc od najdeniot user

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // passwordot koj kako string e ispraten preku DTO

        // Bidejki hashovite se kako bytes/niza mora vo for ciklus

        for(var i = 0; i<computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid password !");
            }
        }

        return user.toDto(tokenService); // extension na static metoda 
    }


    private async Task<bool> EmailExist(string email)
    {
        return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());
    }

}