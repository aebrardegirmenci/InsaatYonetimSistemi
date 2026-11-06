using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Expense
{
    public int ExpenseId { get; set; }

    public int ProjectId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly Date { get; set; }

    public string? Description { get; set; }

    public virtual Project Project { get; set; } = null!;
}
