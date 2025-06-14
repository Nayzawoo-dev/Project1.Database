using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj3Database.Models;

namespace Prj3Api.Features.Wallet.CheckBalance
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckBalanceController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CheckBalanceController(AppDbContext context)
        {

            _context = context;
        }
        [HttpPost]
        public IActionResult Execute(CheckBalanceRequestModel requestmodel)
        {
            CheckBalanceResponseModel model;
            if (string.IsNullOrEmpty(requestmodel.MobileNo))
            {
                model = new CheckBalanceResponseModel()
                {
                    Message = "Mobile Number is required."
                };
                goto Results;
            }
            var CheckItem = _context.TblWallets.Where(x => x.MobilNo == requestmodel.MobileNo).FirstOrDefault();
            if (CheckItem is null)
            {
                model = new CheckBalanceResponseModel()
                {
                    Message = "Mobile No does not registers."
                };
                goto Results;
            }

            var lst = _context.TblTransactions.Where(x => (x.FromMobileNo == requestmodel.MobileNo || x.ToMobileNo == requestmodel.MobileNo))
                .OrderByDescending(x => x.TransactionDate)
                .Take(5)
                .ToList();
            var list = lst.Select(x => new CheckBalanceTransactionHistoryModel
            {
                TransactionDate = x.TransactionDate,
                Amount = x.Amount,
                FromMobileNo = x.FromMobileNo,
                ToMobileNo = x.ToMobileNo,
                TransactionNo = x.TransactionNo,
            }).ToList();

            model = new CheckBalanceResponseModel()
            {
                Balance = CheckItem.Balance,
                IsSuccess = true,
                Message = $"Your Amount is {CheckItem.Balance}",
                TransactionList = list 
            };
        Results:
            return Ok(model);
        }
    }
}
