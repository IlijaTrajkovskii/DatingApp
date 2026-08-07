
using backendAPI.Data;
using backendAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendAPI.Controllers;


 // localhost:5001/api/members
public class MembersController(AppDbContext context) : BaseApiController // inicijalizirame konstruktor so dbcontext instanca
{

    //endpoints 

    [HttpGet] // na get baranje na /api/members gi vrakja site korisnici
    public async Task<ActionResult<IReadOnlyList<AppUser>>> getMembers()
    {
        var members = await context.Users.AsNoTracking().ToListAsync(); // async await 
        // asNoTracking () se koristi koga samo vcituvame podatoci bez da gi menuvame (efficiency)


        return members;
    }


    [HttpGet("{id}")] // api/members/{id}   id=ilija-id
    public async Task<ActionResult<AppUser>> getMember(string id)
    {
        var member =  await context.Users.FindAsync(id);   //find prebaruva po PK 
        

        if(member == null) return NotFound();

        return member;

    }



}
