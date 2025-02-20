using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int TypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual VariantType Type { get; set; } = null!;
}
