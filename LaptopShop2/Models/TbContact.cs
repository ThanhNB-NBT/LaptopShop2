using System;
using System.Collections.Generic;

namespace LaptopShop2.Models;

public partial class TbContact
{
    public int ContactId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Message { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? IsRead { get; set; }
}
