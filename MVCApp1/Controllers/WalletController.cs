using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Stripe;

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
            requestmodel.Balance = 0;
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
            string query1 = @"select * from Tbl_Wallet where MobileNo = @MobileNo";
            string pattern = @"^(09|\+959)\d{7,9}$";
            if (!Regex.IsMatch(requestmodel.MobileNo, pattern))
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Your Mobile No Is Invalid";
                return View("CreateIndex");
            }
            var res = await connection.QueryFirstOrDefaultAsync<WalletModel>(query1, new WalletModel
            {
                MobileNo = requestmodel.MobileNo,
            });
            if (res != null)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Your Mobile No Is Already Register!";
                return View("CreateIndex");
            }
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
            string query1 = @"select * from Tbl_Wallet where WalletId = @WalletId";
            var result = await connection.QueryAsync<WalletModel>(query1, new WalletModel
            {
                WalletId = id,
            });
            var list = result.ToList();
            if (list.Count is 0)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Cannot Delete";
                return RedirectToAction("Index");
            }
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

        [ActionName("Deposit")]
        public IActionResult Deposit()
        {
            return View("Deposit");
        }

        [HttpPost]
        [ActionName("Deposit")]
        public async Task<IActionResult> Deposit(DepositModel requestmodel)
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query1 = @"select * from Tbl_Wallet where MobileNo = @MobileNo";
            var res = await connection.QueryFirstOrDefaultAsync<WalletModel>(query1, new WalletModel
            {
                MobileNo = requestmodel.MobileNo,
            });
            if (res is null)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Your Mobile No Is Not Register";
                goto Deposit;
            }
            if (requestmodel.Amount <= 0)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Amount Must Be Greater Than 0";
                goto Deposit;
            }
            res.Balance += requestmodel.Amount;
            string query2 = @"UPDATE [dbo].[Tbl_Wallet]
   SET
      [Balance] = @Balance
 WHERE MobileNo = @MobileNo";
            var result = await connection.ExecuteAsync(query2, new WalletModel
            {
                MobileNo = requestmodel.MobileNo,
                Balance = res.Balance,
            });
            TempData["isSuccess"] = true;
            TempData["message"] = "Deposit Successful";
            if (result is 1)
            {
                string query3 = @"INSERT INTO [dbo].[Tbl_WalletHistory]
           ([MobileNo]
           ,[TransactionType]
           ,[Amount]
           ,[Date])
     VALUES
           (@MobileNo
           ,@TransactionType
           ,@Amount
           ,@Date)";
                int results = await connection.ExecuteAsync(query3, new WalletHistory
                {
                    MobileNo = requestmodel.MobileNo,
                    TransactionType = "Deposit",
                    Amount = requestmodel.Amount,
                    Date = DateTime.Now,
                });
                goto Result;
            }
            connection.Close();
        Result:
            return RedirectToAction("Index");
        Deposit:
            return View("Deposit");
        }




    }

    public class WalletHistory
    {
        public string MobileNo { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }

    public class DepositModel
    {
        public string MobileNo { get; set; }
        public decimal Amount { get; set; }
    }
}