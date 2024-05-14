using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbAccount
{
    public int AccountId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Avatar { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public virtual TbRole? Role { get; set; }
}
