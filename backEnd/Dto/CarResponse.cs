namespace backEnd.Dto
{
    public class CarResponse
    {
        public string Patent { get; set; } = null!;

        public string? Type { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }
        
        public bool Garage { get; set; }

        public DateTime AdmissionDate { get; set; }

        public DateTime DischargeDate { get; set; }

        public string? State { get; set; }

        public string? Format { get; set; }

        public decimal Amount { get; set; }

        public string? Location { get; set; }
    }
}
