using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbRole
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbAccount> TbAccounts { get; set; } = new List<TbAccount>();
}
