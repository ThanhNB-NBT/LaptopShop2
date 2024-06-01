using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbOrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual TbOrder Order { get; set; } = null!;

    public virtual TbProduct? Product { get; set; }
}
