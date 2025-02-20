using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class BannerSlide
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Href { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? BannerId { get; set; }

    public virtual Banner? Banner { get; set; }
}
