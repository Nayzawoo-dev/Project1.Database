using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project5Database.Models;

namespace Project5Api.UCSH.StudentRegister
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentRegisterController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public StudentRegisterController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public IActionResult Execute(StudentRequestModel requestModel)
        {
            StudentResponseModel model;
            if (string.IsNullOrEmpty(requestModel.Name))
            {
                model = new StudentResponseModel()
                {
                    Message = "Your Name Is Required"
                };
                goto Results;
            }
            TblStudent student = new TblStudent()
            {
                Name = requestModel.Name,
            };
            TblAttendend tblAttendend = new TblAttendend()
            {
                Name = requestModel.Name,
                AttendendMark = 0
            };
            _appDbContext.TblStudents.Add(student);
            _appDbContext.SaveChanges();
            _appDbContext.TblAttendends.Add(tblAttendend);
            _appDbContext.SaveChanges();
            model = new StudentResponseModel()
            {
                Name = requestModel.Name,
                Success = true,
                Message = "Student Register Successful!"
            };
        Results:
            return Ok(model);
        }

        [HttpDelete]
        public IActionResult Delete(StudentDeleteRequestModel requestModel)
        {
            StudentResponseModel model;
            if (string.IsNullOrEmpty(requestModel.RollNo))
            {
                model = new StudentResponseModel()
                {
                    Message = "Roll No is required"
                };
                goto Results;
            }
            var studentItem = _appDbContext.TblStudents.Where(x => x.RollNo == requestModel.RollNo).FirstOrDefault();
            if(studentItem is null)
            {
                model = new StudentResponseModel()
                {
                    Message = "Your Student is Not Register"
                };
                goto Results;
            }
            _appDbContext.TblStudents.Remove(studentItem);
            _appDbContext.SaveChanges();
            var attendedItem = _appDbContext.TblAttendends.Where(x => x.RollNo == requestModel.RollNo).FirstOrDefault();
            _appDbContext.TblAttendends.Remove(attendedItem);
            _appDbContext.SaveChanges();
            model = new StudentResponseModel()
            {
                Success = true,
                Message = "Your Student is Delete Successful"
            };
        Results:
            return Ok(model);
        }
    }
}