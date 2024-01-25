using System;
using System.Collections.Generic;

namespace backEnd.Models;

public partial class Car
{
    public Guid IdCar { get; set; }

    public string Patent { get; set; } = null!;

    public int? Type { get; set; }

    public int? Brand { get; set; }

    public string? Model { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    public int? State { get; set; }

    public decimal Amount { get; set; }

    public string? Location { get; set; }

    public bool Garage { get; set; }

    public int? Format { get; set; }

    public virtual Brand? BrandNavigation { get; set; }

    public virtual PaymentFormat? FormatNavigation { get; set; }

    public virtual PaymentStatus? StateNavigation { get; set; }

    public virtual CarType? TypeNavigation { get; set; }
}
