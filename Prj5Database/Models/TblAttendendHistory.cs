using System;
using System.Collections.Generic;

namespace Prj6Database.Models;

public partial class TblAttendendHistory
{
    public int AttendendHistoryId { get; set; }

    public string Name { get; set; } = null!;

    public string RollNo { get; set; } = null!;

    public DateTime DateTime { get; set; }
}
