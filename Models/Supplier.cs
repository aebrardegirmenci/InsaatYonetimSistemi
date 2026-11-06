using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
