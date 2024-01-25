using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class PaymentFormat
{
    public int Id { get; set; }

    public string Format { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
