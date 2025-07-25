﻿using System;
using System.Collections.Generic;

namespace Prj6Database.Models;

public partial class TblStudentTable
{
    public int StudentId { get; set; }

    public string? RollNo { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }
}
