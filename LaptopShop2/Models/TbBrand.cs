using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbBrand
{
    public int BrandId { get; set; }

    public string? Name { get; set; }

    public string? Banner { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbProduct> TbProducts { get; set; } = new List<TbProduct>();
}
