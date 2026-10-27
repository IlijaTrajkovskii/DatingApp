
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
public class MembersController(IMemberRepository memberRepository, IPhotoService photoService) : BaseApiController // inicijalizirame konstruktor so dbcontext instanca
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

    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> UploadPhoto([FromForm] IFormFile file)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId()); // go zimame memberot zaedno so se Photos i User

        if(member == null) return BadRequest("Cannot update Member");

        var result = await photoService.UploadPhotoAsync(file);  //uploadnuva na Cloudinary

        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId,
            MemberId = User.GetMemberId()
        };

        if(member.ImageUrl == null) // ako nema slika (prva slika) MAIN IMAGE
        {
            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;
        }

        member.Photos.Add(photo);

        if(await memberRepository.SaveAllAsync()) return photo;

        return BadRequest("Problem uploading photo!");

    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());

        if(member == null) return BadRequest("Cannot get the member from token!");

        var mainPhoto = member.Photos.SingleOrDefault(x => x.Id == photoId);

        if(mainPhoto == null || member.ImageUrl == mainPhoto.Url)
        {
            return BadRequest("Cannot set this image as a main image / Image is already set as main !");
        }

        member.ImageUrl = mainPhoto.Url;           // setting it as a main image
        member.User.ImageUrl = mainPhoto.Url;

        if(await memberRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Problem saving main photo");

       

    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var member = await memberRepository.GetMemberForUpdate(User.GetMemberId());

        if(member == null) return BadRequest("Cannot get member from token");

        var photo = member.Photos.SingleOrDefault(photo => photo.Id == photoId);

        if(photo == null || photo.Url == member.ImageUrl)
        {
            return BadRequest("Cannot delete the main photo / photo is null");
        }

        // ako e objavena na cloudinary
        if(photo.PublicId != null) // znaci e objavena na Cloudinary i treba da se izbrise
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);

            if(result.Error != null) return BadRequest(result.Error.Message);
        }

        member.Photos.Remove(photo);  // delete from database

        if(await memberRepository.SaveAllAsync()) return Ok();

        return BadRequest("Failed to delete the photo");

    }


}
