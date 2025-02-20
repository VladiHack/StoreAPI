﻿using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool IsTitleImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product? Product { get; set; }
}
