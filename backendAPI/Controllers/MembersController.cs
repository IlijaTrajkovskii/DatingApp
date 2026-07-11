
using backendAPI.Data;
using backendAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendAPI.Controllers;


[Route("api/[controller]")] // localhost:5001/api/members
[ApiController]
public class MembersController(AppDbContext context) : ControllerBase // inicijalizirame konstruktor so dbcontext instanca
{

    //endpoints 

    [HttpGet] // na get baranje na /api/members gi vrakja site korisnici
    public async Task<ActionResult<IReadOnlyList<AppUser>>> getMembers()
    {
        var members = await context.Users.ToListAsync();  // async await 

        return members;
    }


    [HttpGet("{id}")] // api/members/{id}   id=ilija-id
    public async Task<ActionResult<AppUser>> getMember(string id)
    {
        var member =  await context.Users.FindAsync(id);
        //find prebaruva po PK 


        if(member == null) return NotFound();

        return member;

    }






}
