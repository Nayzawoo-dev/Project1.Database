using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace MVC2.Controllers
{
    public class WalletController : Controller
    {
        private readonly SqlConnectionStringBuilder _connection;

        public WalletController(IConfiguration configuration)
        {
            _connection = new SqlConnectionStringBuilder(configuration.GetConnectionString("DbConnection"));
        }

        [ActionName("Index")]
        public IActionResult WalletIndex()
        {
            return View("WalletList");
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> WalletList()
        {
            try
            {
                using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
                string query = "select * from Tbl_Wallet";
                connection.Open();
                var res = await connection.QueryAsync<WalletModel>(query);
                var result = res.ToList();
                return Json(new { isSuccess = "true", message = "Successful", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = "false", message = ex.ToString() });
            }
        }

        [ActionName("Create")]
        public IActionResult WalletCreate()
        {
            return View("WalletCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> WalletSave(WalletModel requestModel)
        {
            try
            {
                requestModel.Balance = 0;
                if (string.IsNullOrEmpty(requestModel.WalletUserName))
                {
                    return Json(new {isSuccess = false,message = " User Name Field Is Required"});
                }
                string query = @"INSERT INTO [dbo].[Tbl_Wallet]
           ([WalletUserName]
           ,[FullName]
           ,[MobileNo]
           ,[Balance])
     VALUES
           (@WalletUserName
           ,@FullName
           ,@MobileNo
           ,@Balance)";
                using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
                connection.Open();
                var res = await connection.ExecuteAsync(query, new WalletModel
                {
                    WalletUserName = requestModel.WalletUserName,
                    FullName = requestModel.FullName,
                    MobileNo = requestModel.MobileNo,
                    Balance = requestModel.Balance
                });
                connection.Close();
                return Json(new { isSuccess = "Success", message = "Complete" });
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = "false", message = ex.ToString() });
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> WalletDelete(WalletModel requestmodel)
        {
            try
            {
                string query = @"DELETE FROM [dbo].[Tbl_Wallet]
                             WHERE WalletId = @WalletId";
                using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
                connection.Open();
                var res = await connection.ExecuteAsync(query, requestmodel);
                connection.Close();
                if (res <= 0)
                {
                    return Json(new { IsSuccess = false, Message = "Delete Fail" });
                }
                return Json(new { IsSuccess = true, Message = "Delete Complete" });
            }
            catch(Exception ex)
            {
                return Json(new {IsSuccess = false, Message = ex.ToString() });
            }
        }
        [ActionName("Edit")]
        public IActionResult WalletEdit()
        {
            return View("WalletEdit");
        }
        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> WalletEdit(WalletModel requestmodel)
        {
            try
            {
                string query = "select * from Tbl_Wallet where WalletId = @WalletId";
                using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
                connection.Open();
                var res = await connection.QueryFirstOrDefaultAsync<WalletModel>(query, requestmodel);
                connection.Close();
                return Json(new { isSuccess = true, data = res });
            }
            catch(Exception ex) 
            { 
                return Json(new { isSuccess = false, message = ex.ToString() });
            }
        }
        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> WalletUpdate(WalletModel requestmodel)
        {
            try
            {
                string query = @"UPDATE [dbo].[Tbl_Wallet]
   SET [WalletUserName] = @WalletUserName
      ,[FullName] = @FullName
 WHERE WalletId = @WalletId";
                using IDbConnection connection = new SqlConnection(_connection.ConnectionString);
                connection.Open();
                var res = await connection.ExecuteAsync(query, requestmodel);
                connection.Close();
                if (res > 0)
                {
                    return Json(new { isSuccess = true, message = "Update Complete" });
                }
                return Json(new { isSuccess = false, message = "Update Fail" });
            }
            catch(Exception ex)
            {
                return Json(new {isSuccess = false, message = ex.ToString() });
            }
        }
    }

    public class WalletModel
    {
        public int WalletId { get; set; }
        public string WalletUserName { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public decimal Balance { get; set; }
    }
}
