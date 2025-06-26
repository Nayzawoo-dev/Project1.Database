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
            return View("WalletIndex", lst.ToList());
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
            var lst = await connection.ExecuteAsync(query, requestmodel);
            bool isSuccess = lst > 0;
            string message = isSuccess ? "Success" : "Failed";
            TempData["isSuccess"] = isSuccess;
            TempData["message"] = message;
            connection.Close();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Login")]
        public async Task<IActionResult> LoginIndex(WalletModel requestmodel)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"select * from Tbl_Wallet where WalletUserName = @WalletUserName and MobileNo = @MobileNo";
            var lst = await connection.QueryAsync<WalletModel>(query, requestmodel);
            var list = lst.ToList();
            connection.Close();
            bool isSuccess = list.Count > 0;
            string message = isSuccess ? "Login Successful" : "Incorrect Username or Mobile No";
            TempData["isSuccess"] = isSuccess;
            TempData["message"] = message;
            return View("LoginIndex", list);
        }

        [ActionName("Edit")]
        public async Task<IActionResult> WalletEdit(int id)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"SELECT [WalletId]
      ,[WalletUserName]
      ,[FullName]
      ,[MobileNo]
      ,[Balance]
  FROM [dbo].[Tbl_Wallet] where WalletId = @WalletId";
            var model = await connection.QueryFirstOrDefaultAsync<WalletModel>(query, new WalletModel
            {
                WalletId = id
            });
            if (model is null)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Your Wallet Is Not Register!";
                return RedirectToAction("Index");
            }
            connection.Close();
            return View("WalletEdit", model);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> WalletUpdate(int id, WalletModel requestmodel)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"UPDATE [dbo].[Tbl_Wallet]
   SET [WalletUserName] = @WalletUserName
      ,[FullName] = @FullName
      ,[MobileNo] = @MobileNo
      ,[Balance] = @Balance
 WHERE WalletId = @WalletId";
            var model = new WalletModel
            {
                WalletId = id,
                WalletUserName = requestmodel.WalletUserName,
                FullName = requestmodel.FullName,
                MobileNo = requestmodel.MobileNo,
                Balance = requestmodel.Balance,
            };
            int result = await connection.ExecuteAsync(query, model);
            if (result is 0)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Your Wallet User Is Not Register";
                return RedirectToAction("Index");
            }
            connection.Close();
            TempData["isSuccess"] = true;
            TempData["message"] = "Your Update Successful!";
            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> WalletDelete(int id)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"DELETE FROM [dbo].[Tbl_Wallet]
      WHERE WalletId = @WalletId";
            int res = await connection.ExecuteAsync(query, new WalletModel
            {
                WalletId = id,
            });
            connection.Close();
            TempData["isSuccess"] = true;
            TempData["message"] = "Delete Successful";
            return RedirectToAction("Index");
        }


    }
}