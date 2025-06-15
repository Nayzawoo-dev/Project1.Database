using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project5Database.Models;

namespace Project5Api.UCSH.RollCallHistory
{
    [Route("api/[controller]")]
    [ApiController]
    public class RollCallHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RollCallHistoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            RollCallHistroyResponseModel model;
            var list = _context.TblAttendendHistories.ToList();
            var lst = list.Select(x => new RollCallHistoryList
            {
                RollNo = x.RollNo,
                Name = x.Name,
                DateTime = x.DateTime,
            }).ToList();

            model = new RollCallHistroyResponseModel
            {
                Success = true,
                Message = "This is All Student Roll Call List",
                rollCallHistoryLists = lst

            };
            return Ok(model);
        }

        [HttpPost("RollCallWithRollNo")]
        public IActionResult Get(RollCallHistroyRequestModel requestModel)
        {
            RollCallHistroyResponseModel model;
            if (string.IsNullOrEmpty(requestModel.RollNo))
            {
                model = new RollCallHistroyResponseModel
                {
                    Message = "Roll No Is Required"
                };
                goto Results;
            }
            var list = _context.TblStudents.Where(x => x.RollNo == requestModel.RollNo).FirstOrDefault();
            if(list is null)
            {
                model = new RollCallHistroyResponseModel
                {
                    Message = "Student is Not Register"
                };
                goto Results;
            }
            var lis = _context.TblAttendendHistories.Where(x => x.RollNo == requestModel.RollNo).ToList();
            var lst = lis.Select(x => new RollCallHistoryList
            {
                RollNo = x.RollNo,
                Name = x.Name,
                DateTime = x.DateTime,
            }).ToList();
            model = new RollCallHistroyResponseModel()
            {      
                rollCallHistoryLists = lst,
                Success = true,
                Message = $"This Roll No - {requestModel.RollNo} Roll Call List"
            };
        Results:
            return Ok(model);
        }
    }
}
