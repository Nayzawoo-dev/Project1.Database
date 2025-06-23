using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MVCApp1.Controllers
{
    public class WalletController : Controller
    {
        private readonly SqlConnectionStringBuilder _connection = new SqlConnectionStringBuilder()
        {
            DataSource = "DELL",
            InitialCatalog = "MiniWallet",
            UserID = "SA",
            Password = "root",
            TrustServerCertificate = true
        };
        [HttpGet]
        [ActionName("Index")]
        public async Task<IActionResult> WallletIndex()
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            var lst = await connection.QueryAsync<WalletModel>("select * from Tbl_Wallet");
            connection.Close();
            return View("WalletIndex",lst.ToList());
        }

        
        [ActionName("Create")]
        public IActionResult CreateIndex()
        {
            return View("CreateIndex");
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> CreateIndex(WalletModel requestmodel)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"INSERT INTO [dbo].[Tbl_Wallet]
           ([WalletUserName]
           ,[FullName]
           ,[MobileNo]
           ,[Balance])
     VALUES
           (@WalletUserName,
		   @FullName,
		   @MobileNo,
		   @Balance)";
            var lst = await connection.ExecuteAsync(query,requestmodel);
            bool isSuccess = lst > 0;
            string message = isSuccess ? "Success" : "Failed";
            TempData["isSuccess"] = isSuccess;
            TempData["message"] = message;
            connection.Close();
            return RedirectToAction("Index");
        }
    }

    public class WalletModel
    {
        public string WalletUserName { get; set; }

        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public decimal Balance {  get; set; }
    }
}
