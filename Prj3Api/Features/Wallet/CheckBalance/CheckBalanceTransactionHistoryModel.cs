namespace Prj3Api.Features.Wallet.CheckBalance
{
    public class CheckBalanceTransactionHistoryModel
    {
        public string TransactionNo { get; set; } = null!;

        public string FromMobileNo { get; set; } = null!;

        public string ToMobileNo { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
