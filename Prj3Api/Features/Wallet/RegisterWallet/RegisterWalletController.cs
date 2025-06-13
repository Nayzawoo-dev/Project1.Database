using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Prj3Database.Models;

namespace Prj3Api.Features.Wallet.RegisterWallet
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterWalletController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RegisterWalletController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Execute(RegisterWalletRequestModel request)
        {
            RegisterWalletResponseModel model;
            #region Required Field Check
            if (string.IsNullOrEmpty(request.WalletUserName))
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Wallet User Name is required."
                };
                goto Results;
            }

            if (string.IsNullOrEmpty(request.FullName))
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Full Name is required."
                };
                goto Results;
            }

            if (string.IsNullOrEmpty(request.MobilNo))
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Mobile No is required."
                };
                goto Results;
            }

            if (request.MobilNo.Count() > 12)
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Mobile No is Invalid."
                };
                goto Results;
            }

            if(!Regex.IsMatch(request.MobilNo, @"^\d{1,11}$"))
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Mobile No is Invalid"
                };
                goto Results;
            }

            var itemWallet = _context.TblWallets.Where(x => x.WalletUserName == request.WalletUserName).FirstOrDefault();
            if(itemWallet is not null)
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Wallet User Name is already register"
                };
                goto Results;
            }

            itemWallet = _context.TblWallets.Where(x => x.MobilNo == request.MobilNo).FirstOrDefault();
            if(itemWallet is not null)
            {
                model = new RegisterWalletResponseModel()
                {
                    Message = "Mobile Number is already registers"
                };
                goto Results;
            }


            #endregion

            #region Register
            TblWallet item = new TblWallet()
            {
                Balance = 0,
                WalletUserName = request.WalletUserName,
                FullName = request.FullName,
                MobilNo = request.MobilNo,
            };
            _context.TblWallets.Add(item);
            _context.SaveChanges();
            #endregion

            model = new RegisterWalletResponseModel()
            {
                WalletId = item.WalletId,
                WalletUserName = item.WalletUserName,
                FullName = item.FullName,
                MobilNo = item.MobilNo,
                IsSuccess = true,
                Message = "Success"
            };
        Results:
            return Ok(model);
        }
    }
}
