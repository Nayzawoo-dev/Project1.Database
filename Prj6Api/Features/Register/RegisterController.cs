using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj6Database.Models;

namespace Prj6Api.Features.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RegisterController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Execute(RequestModel requestmodel)
        {
            ResponseModel model;
            if (string.IsNullOrEmpty(requestmodel.Name))
            {
                model = new ResponseModel()
                {
                    Message = "Name Field is Required."
                };
                goto Results;
            }

            if(requestmodel.age < 18)
            {
                model = new ResponseModel()
                {
                    Message = "Your Age is Invalid"
                };
                goto Results;
            }
            TblStudentTable item = new TblStudentTable()
            {
                Name = requestmodel.Name,
                Age = requestmodel.age
            };
            _context.TblStudentTables.Add(item);
            _context.SaveChanges();
            model = new ResponseModel()
            {
               Success = true,
               Message = "Successfully Register."
            };
        Results:
            return Ok(model);
        }
    }
}
