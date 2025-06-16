using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Prj6Database.Models;

namespace Prj6Api.Features.Delete
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : Controller
    {
        private readonly AppDbContext _context;
        public DeleteController(AppDbContext context)
        {
            _context = context;
        }
        [HttpDelete("{RollNo}")]

        public IActionResult Execute(string RollNo)
        {
            ResponseModel model;
            if (string.IsNullOrEmpty(RollNo))
            {
                model = new ResponseModel()
                {
                    Message = "Roll No Field Is Required!"
                };
                goto Results;
            }

            var item = _context.TblStudentTables.Where(x => x.RollNo == RollNo).FirstOrDefault();   
            if(item is null)
            {
                model = new ResponseModel()
                {
                    Message = "This Roll No Is Not Register"
                };
                goto Results;
            }
            _context.TblStudentTables.Remove(item);
            _context.SaveChanges();
            model = new ResponseModel()
            {
                Success = true,
                Message = "Delete Success"
            };
        Results:
            return Ok(model);
        }

    }
}
