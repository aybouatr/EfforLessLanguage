using Microsoft.AspNetCore.Mvc;

namespace UserProfileService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    [HttpGet("{userId}")]
    public IActionResult GetUserProfile(int userId)
    {
        return Ok(new
        {
            Id = userId,
            LanguageLevel = "B2"
        });
    }
}