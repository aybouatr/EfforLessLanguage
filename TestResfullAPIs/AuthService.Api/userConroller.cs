using Microsoft.AspNetCore.Mvc;
using LayerBusnessLogic;

namespace authservice.Api.Controllers
{
    [Route("api/UserController")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id}", Name = "GetUser")]
        [ProducesResponseType(typeof(ClsPerson), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int id = 1)
        {
            var user = ClsPerson.Find(id);

            if (user != null)
            {
                JsonResult jsonResult = new JsonResult(new
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                });

                return Ok(jsonResult);
            }

            return NotFound();
        }

        [HttpGet("GetSumTwoNumber", Name = "GetSumTwoNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetSumTwoNumber(int a, int b)
        {
            if (a < 0 || b < 0)
            {
                return BadRequest("Both numbers must be non-negative.");
            }

            return  Ok(a + b);
        }


        [HttpGet("GetSubTwoNumber", Name = "GetSubTwoNumber")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetSubTwoNumber(int a, int b)
        {
            if (a < 0 || b < 0)
            {
                return BadRequest("Both numbers must be non-negative.");
            }

            return  Ok(a - b);
        }
    
    }
}