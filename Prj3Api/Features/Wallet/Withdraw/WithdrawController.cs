using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj3Api.Features.Wallet.Deposit;
using Prj3Api.Features.Wallet.Withdraw;
using Prj3Database.Models;


[Route("api/[controller]")]
[ApiController]
public class WithdrawController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly decimal _min_amount;
    private readonly IConfiguration _configuration;
    public WithdrawController(AppDbContext context,IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _min_amount = Convert.ToDecimal(_configuration.GetSection("MinAmount").Value);
    }

    [HttpPost]
    public IActionResult Execute(WithdrawRequestModel requestmodel)
    {
        WithdrawResponseModel model;



        var depositItem = _context.TblWallets.Where(x => x.MobilNo == requestmodel.MobileNo).FirstOrDefault();
        if (depositItem is null)
        {
            model = new WithdrawResponseModel()
            {
                Message = "Wallet User is not register."
            };
            goto Results;
        }

        if (requestmodel.Amount <= 0)
        {
            model = new WithdrawResponseModel()
            {
                Message = "Deposit Amount Must Be Greater Than 0."
            };
            goto Results;
        }
        decimal oldBalance = depositItem.Balance;
        if(requestmodel.Amount > oldBalance - _min_amount)
        {
            model = new WithdrawResponseModel()
            {
                Message = $"Isufficiant Amount.MinAmount must be {_min_amount.ToString("n2")}"
            };
            goto Results;
        }
        decimal newBalance = depositItem.Balance - requestmodel.Amount;
        depositItem.Balance = newBalance;
        _context.SaveChanges();

        TblWalletHistory item = new TblWalletHistory()
        {
            MobileNo = requestmodel.MobileNo,
            TransactionType = "Withdraw",
            Amount = requestmodel.Amount,
        };
        _context.TblWalletHistories.Add(item);
        _context.SaveChanges();
        model = new WithdrawResponseModel()
        {
            IsSuccess = true,
            OldBalance = oldBalance,
            NewBalance = newBalance,
            Message = $"Withdraw Amount - {requestmodel.Amount}"
        };
    Results:
        return Ok(model);
    }
}
