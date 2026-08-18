using Microsoft.AspNetCore.Mvc;

namespace backendAPI.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("auth")]   //api/buggy/auth
    public IActionResult GetAuth()
    {
        return Unauthorized();
    }

    [HttpGet("not-found")]     //api/buggy/not-found
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("Server-error")]
    public IActionResult GetServerError()
    {
       throw new Exception("This is a server error");
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("This is a bad request!");
    }
    
}