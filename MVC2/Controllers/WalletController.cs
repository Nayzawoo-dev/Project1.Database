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
