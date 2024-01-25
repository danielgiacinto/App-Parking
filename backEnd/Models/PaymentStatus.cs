using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class PaymentStatus
{
    public int IdStatus { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
