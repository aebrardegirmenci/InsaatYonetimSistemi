using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Income
{
    public int IncomeId { get; set; }

    public int ProjectId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public string? Description { get; set; }

    public virtual Project Project { get; set; } = null!;
}
