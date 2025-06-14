namespace Prj3Api.Features.Wallet.CheckBalance
{
    public class CheckBalanceResponseModel : ResponseModel
    {
        public decimal Balance { get; set; }          
        public List<CheckBalanceTransactionHistoryModel>? TransactionList { get; set; }  
        
    }
}
