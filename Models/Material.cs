using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Material
{
    public int MaterialId { get; set; }

    public int ProjectId { get; set; }

    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }

    public DateOnly Date { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Supplier Supplier { get; set; } = null!;
}
