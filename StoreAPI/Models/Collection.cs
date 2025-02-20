using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class Collection
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int? StoreId { get; set; }

    public virtual ICollection<CollectionProduct> CollectionProducts { get; set; } = new List<CollectionProduct>();

    public virtual StoreOption? Store { get; set; }
}
