using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbCategoryProduct
{
    public int CategoryProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbProduct> TbProducts { get; set; } = new List<TbProduct>();
}
