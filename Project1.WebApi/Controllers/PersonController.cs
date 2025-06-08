using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Database.Models;
using Project1.Domain.Features;

namespace Project1.WebApi.Controllers
{
    [Route("person/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly Personservices _personservices;

        public PersonController(Personservices personservices)
        {
            _personservices = personservices;   
        }

        [HttpPut("{id}")]
        public IActionResult CreateAndUpdatePerson([FromBody] TblWindow window,int id)
        {
            var model = _personservices.UpdateCreatePerson(window, id);
            if(model.IsSuccess == false)
            {
                return BadRequest(model);
            }
            return Ok(model);
        }
    }
}
