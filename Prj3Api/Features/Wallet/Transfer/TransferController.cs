using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj3Database.Models;

namespace Prj3Api.Features.Wallet.Transfer
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly decimal _min_amount;
        public TransferController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _min_amount = Convert.ToDecimal(_configuration.GetSection("MinAmount").Value);

        }
        [HttpPost]
        public IActionResult Execute(TransferRequestModel requestModel)
        {
            TransferResponseModel model;
            if (string.IsNullOrEmpty(requestModel.FromMobileNo))
            {
                model = new TransferResponseModel()
                {
                    Message = "From Mobile No is required"
                };
                goto Results;
            }
            if (string.IsNullOrEmpty(requestModel.ToMobileNo))
            {
                model = new TransferResponseModel()
                {
                    Message = "To Mobile No is required"
                };
                goto Results;
            }
            if (requestModel.Amount <= 0)
            {
                model = new TransferResponseModel()
                {
                    Message = "Amount must be greater than 0"
                };
            }

            var itemFromMobileNo = _context.TblWallets.Where(x => x.MobilNo == requestModel.FromMobileNo).FirstOrDefault();
            if (itemFromMobileNo is null)
            {
                model = new TransferResponseModel()
                {
                    Message = "From Mobile No is not register"
                };
                goto Results;
            }

            var itemToMobileNo = _context.TblWallets.Where(x => x.MobilNo == requestModel.ToMobileNo).FirstOrDefault();
            if (itemToMobileNo is null)
            {
                model = new TransferResponseModel()
                {
                    Message = "To Mobile is not register"
                };
                goto Results;
            }

            if (requestModel.Amount > itemFromMobileNo.Balance - _min_amount)
            {
                model = new TransferResponseModel()
                {
                    Message = "Insufficiant Amount."
                };
                goto Results;
            }
            itemFromMobileNo.Balance -= requestModel.Amount;
            itemToMobileNo.Balance += requestModel.Amount;
            _context.SaveChanges();

            TblTransaction item = new TblTransaction()
            {
                Amount = requestModel.Amount,
                FromMobileNo = requestModel.FromMobileNo,
                ToMobileNo = requestModel.ToMobileNo,
                TransactionDate = DateTime.Now,
                TransactionNo = DateTime.Now.ToString("yyyMMdd_hhmmss_fff"),
                TransactionId = Ulid.NewUlid().ToString()

            };
            _context.TblTransactions.Add(item);
            _context.SaveChanges();

            model = new TransferResponseModel()
            {
                IsSuccess = true,
                Message = "Transfer Success"

            };
        Results:
            return Ok(model);
        }
    }
}
