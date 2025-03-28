using System;
using System.Collections.Generic;

namespace WebApi_04;

public partial class User
{
    public long Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Age { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
