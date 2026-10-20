
using System.Security.Claims;
using backendAPI.Data;
using backendAPI.DTOs;
using backendAPI.Entities;
using backendAPI.Extensions;
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

    [HttpGet("{id}/photos")]  //api/members/id/photos
    public async Task<ActionResult<IReadOnlyList<Photo>>> GetMemberPhotos(string id)
    {
        return Ok(await memberRepository.GetPhotosForMemberAsync(id));
    }


    //pri update na user ne se isprakja id bidjeki preku auth tokenot go dobivame 
    [HttpPut]  //api/members PUT metoda
    public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
    {
        // User e ClaimsPrincipal (ne e entitetot AppUser)
        var memberId = User.GetMemberId();

        var member = await memberRepository.GetMemberForUpdate(memberId);

        if(member == null) return BadRequest("Could not get the member");

        member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
        member.Description = memberUpdateDto.Description ?? member.Description;
        member.City = memberUpdateDto.City ?? member.City;
        member.Country = memberUpdateDto.Country ?? member.Country;

        member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName; // go updatetirame istovremeno i user entitetot vo pozadina         


        memberRepository.Update(member); // optional -> za da moze da se isprakja prazen memberUpdateDTO objekt i da se pomine requestot

        if(await memberRepository.SaveAllAsync()) return NoContent(); // 204 success

        return BadRequest("Failed to update member!");



    }



}
