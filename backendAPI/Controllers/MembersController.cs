
using backendAPI.Data;
using backendAPI.Entities;
using backendAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendAPI.Controllers;


 // localhost:5001/api/members
[Authorize]
public class MembersController(IMemberRepository memberRepository) : BaseApiController // inicijalizirame konstruktor so dbcontext instanca
{

    //endpoints 

    [HttpGet] // na get baranje na /api/members gi vrakja site korisnici
    public async Task<ActionResult<IReadOnlyList<Member>>> getMembers()
    {
    
        return Ok(await memberRepository.GetMembersAsync());
    }

    
    [HttpGet("{id}")] // api/members/{id}   id=ilija-id
    public async Task<ActionResult<Member>> getMember(string id)
    {
        var member = await memberRepository.GetMemberByIdAsync(id);   //find prebaruva po PK 
        

        if(member == null) return NotFound();

        return member;

    }

    [HttpGet("{id}/photos")]  //members/id/photos
    public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
    {
        return Ok(await memberRepository.GetPhotosForMemberAsync(id));
    }





}
