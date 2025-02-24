using System;
using System.Collections.Generic;

namespace SQLMVC99.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Price { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;
}
