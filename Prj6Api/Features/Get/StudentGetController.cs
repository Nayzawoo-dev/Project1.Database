using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj6Database.Models;

namespace Prj6Api.Features.Get
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentGetController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentGetController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Execute()
        {
            ResponseModel model;
            var item = _context.TblStudentTables.ToList();
            var studentItem = item.Select(x => new StudentList
            {
                RollNo = x.RollNo,
                Name = x.Name,
                Age = x.Age,
            }).ToList();
            model = new GetResponseModel()
            {
                Success = true,
                Message = "Student List",
                Data = studentItem
                
            };
            return Ok(model);
        }

        [HttpGet("{RollNo}")]
        public IActionResult Execute(string RollNo)
        {
            GetResponseModel model;
            if (string.IsNullOrEmpty(RollNo))
            {
                model = new GetResponseModel()
                {
                    Message = "Roll No is Required."
                };
                goto Results;
            }

            var item = _context.TblStudentTables.Where(x => x.RollNo == RollNo).ToList();
            if(item.Count is 0)
            {
                model = new GetResponseModel()
                {
                    Message = "Your Roll No is not register"
                };
                goto Results;
            }
            var studentItem = item.Select(x => new StudentList
            {
                RollNo = x.RollNo,
                Name = x.Name,
                Age = x.Age,
            }).ToList();
            model = new GetResponseModel()
            {
                Success = true,
                Message = "This Is Your Student",
                Data = studentItem
            };
        Results:
            return Ok(model);
        }
    }
}
