using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopShop2.Models;

public partial class TbProduct
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public string? Image { get; set; }

    public DateTime? CreatedDate { get; set; }

    public decimal? Price { get; set; }

    public decimal? Discount { get; set; }

    public int? Quantity { get; set; }

    public bool IsNew { get; set; }

    public bool IsBestSell { get; set; }

    public bool IsActive { get; set; }

    public string? Code { get; set; }

    public string? Origin { get; set; }

    public string? Size { get; set; }

    public string? Weight { get; set; }

    public string? Color { get; set; }

    public string? Material { get; set; }

    public string? CpuCompany { get; set; }

    public string? CpuType { get; set; }

    public string? CpuSpeed { get; set; }

    public string? CpuMaxSpeed { get; set; }

    public int? CpuCore { get; set; }

    public int? CpuProcessor { get; set; }

    public string? RamSize { get; set; }

    public string? RamType { get; set; }

    public string? RamSpeed { get; set; }

    public string? RamSupportMax { get; set; }

    public string? ScreenSize { get; set; }

    public string? ScreenPixel { get; set; }

    public string? ScreenPanel { get; set; }

    public string? CardBrand { get; set; }

    public string? CardModel { get; set; }

    public string? DriveType { get; set; }

    public string? DriveMemory { get; set; }

    public string? ConnectPort { get; set; }

    public string? Wifi { get; set; }

    public string? Bluetooth { get; set; }

    public string? Webcam { get; set; }

    public string? PinType { get; set; }

    public string? PinCapacity { get; set; }

    public string? Os { get; set; }

    public string? Version { get; set; }

    public int? CategoryProductId { get; set; }

    public int? BrandId { get; set; }

    public virtual TbBrand? Brand { get; set; }

    public virtual TbCategoryProduct? CategoryProduct { get; set; }

    public virtual ICollection<TbImage> TbImages { get; set; } = new List<TbImage>();

    public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; } = new List<TbOrderDetail>();

    public virtual ICollection<TbProductReview> TbProductReviews { get; set; } = new List<TbProductReview>();

    [NotMapped]
    public string? CategoryInput { get; set; }
}
