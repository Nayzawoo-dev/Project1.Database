namespace Prj3Api.Features.Wallet.Transfer
{
    public class TransferRequestModel
    {
        public string? FromMobileNo { get; set; }
        public string? ToMobileNo { get; set;}
        public decimal Amount { get; set;}

    }
}
