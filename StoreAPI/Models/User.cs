using System;
using System.Collections.Generic;

namespace StoreAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? StoreId { get; set; }

    public virtual StoreOption? Store { get; set; }
}
