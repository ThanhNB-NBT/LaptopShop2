using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbCategoryNew
{
    public int CategoryNewId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TbNews> TbNews { get; set; } = new List<TbNews>();
}
