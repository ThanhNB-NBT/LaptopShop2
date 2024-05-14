using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbImage
{
    public int ImageId { get; set; }

    public string? ImagePath { get; set; }

    public int? ProductId { get; set; }

    public virtual TbProduct? Product { get; set; }
}
