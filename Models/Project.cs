using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public enum ProjectStatus
{
    DevamEden = 0,
    Tamamlanan = 1,
    Planlanan = 2,
    Beklemede = 3
}

public partial class Project
{
    public int ProjectId { get; set; }

    public int CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public DateOnly StartDate { get; set; }

    public decimal Budget { get; set; }

    public decimal? Area { get; set; }

    public int? FloorCount { get; set; }

    public int? ApartmentCount { get; set; }

    public bool IsActive { get; set; }

    public ProjectStatus Status { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual ICollection<Work> Works { get; set; } = new List<Work>();
}