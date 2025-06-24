using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MVCApp2.Controllers
{
    public class StudentController : Controller
    {
        private readonly SqlConnectionStringBuilder _connection = new SqlConnectionStringBuilder()
        {
            DataSource = "DELL",
            InitialCatalog = "UCSH",
            UserID = "SA",
            Password = "root",
            TrustServerCertificate = true
        };
        [ActionName("Index")]
        public async Task<IActionResult> StudentList()
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            var lst =await connection.QueryAsync<StudentModel>("select * from Tbl_Student");
            connection.Close();
            return View("StudentList",lst.ToList());
        }
     
        [ActionName("Create")]
        public IActionResult StudentCreate()
        {
            return View("StudentCreate");
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> StudentCreate(StudentModel requestmodel)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"INSERT INTO [dbo].[Tbl_Student]
           ([Name])
     VALUES
           (@Name)";
            if (string.IsNullOrEmpty(requestmodel.Name)) 
            {
                bool Success = false;
                string Message = "Username Field Is Required";
                TempData["isSuccess"] = Success;
                TempData["message"] = Message;
                return View("StudentCreate");
            }
            var lst = await connection.ExecuteAsync(query,requestmodel);
            bool isSuccess = lst > 0;
            string message = isSuccess ? "Add New Student Successfully" : "Add New Student Failed";
            connection.Close();
            TempData["isSuccess"] = isSuccess;
            TempData["message"] = message;
            return RedirectToAction("Index");
        }

        [ActionName("LoginUser")]
        public IActionResult LoginUser()
        {
            return View("LoginUser");
        }

        [ActionName("login")]
        public async Task<IActionResult> StudentLogin(StudentModel requestmodel)
        {
            
            bool isSuccess;
            string message;
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"SELECT [StudentId]
      ,[RollNo]
      ,[Name]
  FROM [dbo].[Tbl_Student] where RollNo = @RollNo and Name = @Name";
            if (string.IsNullOrEmpty(requestmodel.Name))
            {
                isSuccess = false;
                message = "Name Field is required";
                TempData["isSuccess"] = isSuccess;
                TempData["message"] = message;
                return View("StudentLogin");
            }

            if (string.IsNullOrEmpty(requestmodel.RollNo))
            {
                isSuccess = false;
                message = "Roll No Field is required";
                TempData["isSuccess"] = isSuccess;
                TempData["message"] = message;
                return View("StudentLogin");
            }

            var lst = await connection.QueryAsync<StudentModel>(query,requestmodel);
            var list = lst.ToList();
            connection.Close();
            isSuccess = list.Count > 0;
            message = isSuccess ? "Login Successful" : "Incorrect Username Or Password";
            if(isSuccess is false)
            {
                TempData["isSuccess"] = isSuccess;
                TempData["message"] = message;
                return View("StudentLogin");
            }
            TempData["isSuccess"] = isSuccess;
            TempData["message"] = message;
            return View("LoginUser",list);
        }
    }

    public class StudentModel
    {
        public string RollNo { get; set; }
        public string Name { get; set; }    
    }
}
