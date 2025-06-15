using System;
using System.Collections.Generic;

namespace Project5Database.Models;

public partial class TblStudent
{
    public int StudentId { get; set; }

    public string? RollNo { get; set; }

    public string Name { get; set; } = null!;
}
