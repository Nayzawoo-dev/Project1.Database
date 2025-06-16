using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj6Database.Models;

namespace Prj6Api.Features.Update
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UpdateController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPatch]
        public IActionResult Execute(UpdateRequestModel requestmodel)
        {
            ResponseModel model;
            if (string.IsNullOrEmpty(requestmodel.RollNo))
            {
                model = new ResponseModel()
                {
                    Message = "Roll No Field Is Required"
                };
                goto Results;
            }

            var item = _context.TblStudentTables.Where(x => x.RollNo == requestmodel.RollNo).FirstOrDefault();
            if(item is null)
            {
                model = new ResponseModel()
                {
                    Message = "Your Student is not Register"
                };
                goto Results;
            }
            item.Age = requestmodel.Age;
            _context.SaveChanges();
            model = new ResponseModel()
            {
                Success = true,
                Message = "Update Age"
            };
        Results:
            return Ok(model);
        }
    }
}
