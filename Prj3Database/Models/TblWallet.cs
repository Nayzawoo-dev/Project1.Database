using System;
using System.Collections.Generic;

namespace Prj3Database.Models;

public partial class TblWallet
{
    public int WalletId { get; set; }

    public string WalletUserName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string MobilNo { get; set; } = null!;

    public decimal Balance { get; set; }
}
