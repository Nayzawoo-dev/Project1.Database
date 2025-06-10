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

        [HttpGet]
        public IActionResult GetPerson()
        {
            var list = _personservices.GetPersons();
            return Ok(list);
        }

        [HttpGet("{pageNo}/{pageSize}")]

        public IActionResult GetPerson(int pageNo, int pageSize)
        {
            var list = _personservices.GetPersons(pageNo, pageSize);
            if (list.IsSuccess == false)
            {
                return BadRequest("Not Found");
            }
            return Ok(list);
        }

        [HttpPost]
        public IActionResult PostPerson([FromBody] TblWindow window)
        {
            var result = _personservices.PostPerson(window);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult CreateAndUpdatePerson([FromBody] TblWindow window, int id)
        {
            var model = _personservices.UpdateCreatePerson(window, id);
            if (model.IsSuccess == false)
            {
                return BadRequest(model);
            }
            return Ok(model);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdatePerson(TblWindow window, int id)
        {
            var result = _personservices.UpdatePerson(window, id);
            if (result.IsSuccess == false)
            {
                return BadRequest(result);
            }
            return Ok(result);


        }

        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id) { 
         var result = _personservices.DeletePerson(id);
         return Ok(result);
        }




    }
}
