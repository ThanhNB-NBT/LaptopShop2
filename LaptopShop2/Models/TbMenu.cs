using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopShop2.Models;

public partial class TbMenu
{
    public int MenuId { get; set; }

    public string MenuName { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public int? MenuLevel { get; set; }

    public int? MenuOrder { get; set; }

    public int? ParentId { get; set; }

    public int? Position { get; set; }

    public bool IsActive { get; set; }
    [NotMapped]
    public string? ParentName { get; set; } = string.Empty;
}
