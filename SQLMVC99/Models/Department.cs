using System;
using System.Collections.Generic;

namespace SQLMVC99.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Manager { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateOnly? CreatedDate { get; set; }
}
