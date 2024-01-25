using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class CarType
{
    public int IdCarType { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
