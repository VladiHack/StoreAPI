using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class Banner
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? StoreId { get; set; }

    public virtual ICollection<BannerSlide> BannerSlides { get; set; } = new List<BannerSlide>();

    public virtual StoreOption? Store { get; set; }
}
