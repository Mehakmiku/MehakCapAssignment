using System;
using System.Collections.Generic;

namespace DB_First_EF_Demo.Models;

public partial class CustomProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
