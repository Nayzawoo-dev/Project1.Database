using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj3Database.Models;

namespace Prj3Api.Features.Wallet.Deposit
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly AppDbContext _context;
        public DepositController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Execute(DepositRequestModel requestModel)
        {
            DepositResponseModel model;
            if (string.IsNullOrEmpty(requestModel.MobileNo))
            {
                model = new DepositResponseModel()
                {
                    Message = "Mobile No is required."
                };
                goto Results;
            }

            if (requestModel.Amount <= 0)
            {
                model = new DepositResponseModel()
                {
                    Message = "Amount must be greater than 0."
                };
                goto Results;
            }
            var depositItem = _context.TblWallets.Where(x => x.MobilNo == requestModel.MobileNo).FirstOrDefault();
            if (depositItem is null)
            {
                model = new DepositResponseModel()
                {
                    Message = "This Mobile No is not register."
                };
                goto Results;

            }

             decimal oldBalance = depositItem.Balance;
             decimal newBalance = depositItem.Balance + requestModel.Amount;
            depositItem.Balance = newBalance;
            _context.SaveChanges();

            TblWalletHistory item = new TblWalletHistory()
            {
                MobileNo = requestModel.MobileNo,
                TransactionType = "Deposit",
                Amount = requestModel.Amount,
            };
            _context.TblWalletHistories.Add(item);
            _context.SaveChanges();
            model = new DepositResponseModel()
            {
                IsSuccess = true,
                OldBalance = oldBalance,
                NewBalance = newBalance,
                Message = $"Deposit Amount {requestModel.Amount.ToString("n2")}"
            };
        Results:
            return Ok(model);
        }
    }
}
