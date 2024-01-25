using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class Brand
{
    public int IdBrand { get; set; }

    public string BrandName { get; set; } = null!;

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}
