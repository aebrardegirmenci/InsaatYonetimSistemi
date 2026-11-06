using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string Job { get; set; } = null!;

    public decimal DailyPay { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Work> Works { get; set; } = new List<Work>();
}
