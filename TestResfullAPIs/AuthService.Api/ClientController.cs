using Microsoft.AspNetCore.Mvc;
using LayerBusnessLogic;
using System.Text.Json;
using System.Runtime.InteropServices.JavaScript;

namespace authservice.Api.Controllers
{
    [Route("api/ClientController")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        [HttpGet("TestGet", Name = "TestGet")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult TestGet()
        {
            return Ok("TestGet method executed successfully.");
        }

        [HttpPost("TestPost", Name = "TestPost")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult TestPost([FromBody] ClsPerson person)
        {
            if (person == null)
            {
                return BadRequest("Person object is null.");    
            }

            string jsonString = "";
            try
            {
               
               jsonString = JsonSerializer.Serialize(person);
               if (string.IsNullOrEmpty(jsonString))
                {
                    return BadRequest("Failed to serialize the person object.");
                }

                // You can add more validation logic here as needed

            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing the request: {ex.Message}");
            }

            return Ok($"TestPost method executed successfully. Received person: {jsonString}");
        }
    }


    
}