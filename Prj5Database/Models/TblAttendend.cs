using System;
using System.Collections.Generic;

namespace Prj6Database.Models;

public partial class TblAttendend
{
    public int AttendendId { get; set; }

    public string? RollNo { get; set; }

    public string Name { get; set; } = null!;

    public int AttendendMark { get; set; }
}
