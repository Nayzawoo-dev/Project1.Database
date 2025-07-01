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

        [ActionName("Withdraw")]
        public IActionResult Withdraw()
        {
            return View("Withdraw");
        }

        [HttpPost]
        [ActionName("Withdraw")]
        public async Task<IActionResult> Withdraw(DepositModel requestmodel)
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
                goto Withdraw;
            }
            if (requestmodel.Amount <= 0)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Amount Must Be Greater Than 0";
                goto Withdraw;
            }
            res.Balance -= requestmodel.Amount;
            if (res.Balance <= 10000)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "You have to leave at least 3000";
                goto Withdraw;
            }
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
            TempData["message"] = "Withdraw Successful";
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
                    TransactionType = "Withdraw",
                    Amount = requestmodel.Amount,
                    Date = DateTime.Now,
                });
                goto Result;
            }
            connection.Close();
        Result:
            return RedirectToAction("Index");
        Withdraw:
            return View("Withdraw");
        }

        [ActionName("History")]
        public async Task<IActionResult> History()
        {
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = "select * from Tbl_WalletHistory";
            var res = await connection.QueryAsync<WalletHistory>(query);
            return View("History", res);
        }

        [ActionName("Transaction")]

        public IActionResult Transaction()
        {
            return View("Transaction");
        }

        [HttpPost]
        [ActionName("Transaction")]
        public async Task<IActionResult> Transaction(TransactionModel requestmodel)
        {
            int num = 0;
            string count = "KBZ" + (++num);
            using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
            connection.Open();
            string query = @"select * from Tbl_Wallet where MobileNo = @MobileNo";
            var model1 = new WalletModel
            {
                MobileNo = requestmodel.FromMobileNo
            };
            var res = await connection.QueryAsync<WalletModel>(query, model1);
            if (res is null)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "From Mobile No Is Not Register!";
                goto Transaction;

            }
            string query1 = @"select * from Tbl_Wallet where MobileNo = @MobileNo";
            var model2 = new WalletModel
            {
                MobileNo = requestmodel.ToMobileNo,
            };
            var res1 = await connection.QueryAsync<WalletModel>(query1,model2);
            if (res1 is null)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "To Mobile No Is Not Register";
                goto Transaction;
            }
            model1.Balance -= requestmodel.Amount;
            if (requestmodel.Amount > 0 || model1.Balance <= 10000)
            {
                TempData["isSuccess"] = false;
                TempData["message"] = "Amount Must Be Greater Than 0 or Must At Least 10000";
                goto Transaction;
            }
            model2.Balance += requestmodel.Amount;
            string query2 = @"UPDATE [dbo].[Tbl_Wallet]
   SET 
      [Balance] = @Balance
   
 WHERE FromMobileNo = @FromMobileNo";
            var result = await connection.ExecuteAsync(query2, new WalletModel
            {
                Balance = model1.Balance,
            });
            string query3 = @"UPDATE [dbo].[Tbl_Wallet]
   SET 
      [Balance] = @Balance
   
 WHERE ToMobileNo = @ToMobileNo";
            var result1 = await connection.ExecuteAsync(query3, new WalletModel
            {
                Balance = model2.Balance,
            });
            string query4 = @"INSERT INTO [dbo].[Tbl_Transaction]
           ([TransactionId]
           ,[TransactionNo]
           ,[FromMobileNo]
           ,[ToMobileNo]
           ,[Amount]
           ,[TransactionDate])
     VALUES
           (<TransactionId, varchar(50),>
           ,<TransactionNo, varchar(50),>
           ,<FromMobileNo, varchar(50),>
           ,<ToMobileNo, varchar(50),>
           ,<Amount, decimal(20,2),>
           ,<TransactionDate, date,>)";
            var list = await connection.ExecuteAsync(query4, new TransactionModel
            {
                TransactionId = DateTime.Now,
                TransactionNo = count,
                FromMobileNo = requestmodel.FromMobileNo,
                ToMobileNo = requestmodel.ToMobileNo,
                TransactionDate = DateTime.Now,
            });
            connection.Close();
            TempData["isSuccess"] = true;
            TempData["message"] = "Transaction Successful!";
        Transaction:
            return View("Transaction");
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

    public class TransactionModel
    {
        public DateTime TransactionId { get; set; }
        public string TransactionNo { get; set; }
        public string FromMobileNo { get; set; }
        public string ToMobileNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}