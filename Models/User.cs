using System;
using System.Collections.Generic;

namespace InsaatYonetimi.Models;

public partial class User
{
    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
