using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class User
{
    public Guid IdUsers { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
