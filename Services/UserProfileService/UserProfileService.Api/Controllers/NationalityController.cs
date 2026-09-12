using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer;
using DataAccesLayer;

namespace NationalityController.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalityController : ControllerBase
    {
        [HttpGet("GetNationalityById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetNationalityById(long id)
        {
            if (nationalitY.GetNationalityById(id, out NationalityDTO? nationality))
            {
                return Ok(nationality);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllNationalities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllNationalities()
        {
            var nationalities = nationalitY.GetAllNationalities();
            if (nationalities == null || nationalities.Count == 0)
            {
                return NotFound(); 
            }
            return Ok(nationalities);
        }


        [HttpPost("AddNewNationality")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddNewNationality([FromBody] NationalityDTO nationalityDTO)
        {
            if (nationalitY.AddNewNationality(ref nationalityDTO))
            {
                return Ok(nationalityDTO);
            }
            else
            {
                return BadRequest("Failed to add new nationality.");
            }
        }

    }
}