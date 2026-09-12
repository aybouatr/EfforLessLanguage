using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer;
using DataAccesLayer;



namespace InterestController.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
       
        [HttpGet("GetInterestByName")]
         [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetInterestByName(string name)
        {
            var interest = Interest.GetInterestByName(name);
            if (interest != null)
            {
                return Ok(interest);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddInterest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddInterest(string name)
        {
            long newId = Interest.AddInterest(name);
            if (newId <= 0)
            {
                return BadRequest("Failed to add interest.");
            }
            return Ok(Interest.GetInterestByName(name));
        }

        [HttpGet("GetAllInterests")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllInterests()
        {
            var interests = Interest.GetAllInterests();
            if (interests == null || interests.Count == 0)
            {
                return NotFound();
            }
            return Ok(interests);
        }
    
    
    }
}