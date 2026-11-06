using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class Work
{
    public int WorkId { get; set; }

    public int WorkerId { get; set; }

    public int ProjectId { get; set; }

    public DateOnly StartDate { get; set; }

    public int DayCount { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Worker Worker { get; set; } = null!;
}
