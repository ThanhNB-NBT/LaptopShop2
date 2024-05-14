using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbProductReview
{
    public int ProductReviewId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ProductId { get; set; }

    public bool? IsActive { get; set; }

    public virtual TbProduct? Product { get; set; }
}
