using System;
using System.Collections.Generic;

namespace WebApi_04;

public partial class Order
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual User User { get; set; } = null!;
}
