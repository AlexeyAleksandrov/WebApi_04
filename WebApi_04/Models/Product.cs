using System;
using System.Collections.Generic;

namespace WebApi_04;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Price { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
