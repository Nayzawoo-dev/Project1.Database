using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project5Database.Models;

namespace Project5Api.UCSH.StudentRollCall
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendedRollCallController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public AttendedRollCallController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public IActionResult Execute(AttendedRequestModel requestmodel)
        {
            AttendedResponseModel model;
            if (string.IsNullOrEmpty(requestmodel.RollNo))
            {
                model = new AttendedResponseModel()
                {
                    Message = "Roll No is required."
                };
                goto Results;
            }
            var AttendendItem = _appDbContext.TblAttendends.Where(x => x.RollNo == requestmodel.RollNo).FirstOrDefault();   
            if(AttendendItem is null)
            {
                model = new AttendedResponseModel()
                {
                    Message = "Your Student is not register"
                };
                goto Results;
            }
            AttendendItem.AttendendMark += 1;
            _appDbContext.SaveChanges();
            TblAttendendHistory history = new TblAttendendHistory()
            {
                RollNo = requestmodel.RollNo,
                DateTime = DateTime.Now,
                Name = AttendendItem.Name,
            };
            _appDbContext.TblAttendendHistories.Add(history);
            _appDbContext.SaveChanges();
            model = new AttendedResponseModel()
            {
                RollNo = requestmodel.RollNo,
                Success = true,
                Message = $"Your Are Mark +1 This Roll No - {requestmodel.RollNo}"
            };
        Results:
            return Ok(model);
        }
    }
}
