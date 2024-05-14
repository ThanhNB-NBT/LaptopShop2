using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbNews
{
    public int NewId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? Image { get; set; }

    public string? Tags { get; set; }

    public int? CategoryNewId { get; set; }

    public bool IsActive { get; set; }

    public virtual TbCategoryNew? CategoryNew { get; set; }
}
