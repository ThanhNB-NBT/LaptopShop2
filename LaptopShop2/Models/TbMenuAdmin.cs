using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbMenuAdmin
{
    public int MenuAdminId { get; set; }

    public string? ItemName { get; set; }

    public string? AreaName { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? Icon { get; set; }

    public int? ItemLevel { get; set; }

    public int? ItemOrder { get; set; }

    public int? ParentId { get; set; }

    public bool IsActive { get; set; }
}
