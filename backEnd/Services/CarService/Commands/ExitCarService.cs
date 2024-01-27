using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarService.Commands
{

    public class ExitCarService
    {
        public class ExitCarCommand : IRequest<CarResponse>
        {
            public string Patent { get; set; } = null!;

            public DateTime DischargeDate { get; set; }

            public int? Format {get; set; }

        }

        public class ExiteCarValidator : AbstractValidator<ExitCarCommand>
        {

            private readonly DbEstacionamientoContext _context;
            public ExiteCarValidator(DbEstacionamientoContext context)
            {
                _context = context;

                RuleFor(c => c.Patent).MinimumLength(6).MaximumLength(7).NotNull()
                .Must(ValidatePatent).WithMessage("La patente no existe en la base de datos.");
                RuleFor(c => c.DischargeDate).NotNull()
                .Must((command, dischargeDate) => ValidateDischargeDate(command.Patent, dischargeDate))
                .WithMessage("La fecha de salida debe ser posterior a la de ingreso.");
                RuleFor(c => c.Format).NotEmpty().NotNull().NotEqual(0);
            }

            private bool ValidateDischargeDate(string patent, DateTime dischargeDate)
            {
                var admissionDate = _context.Cars
                    .Where(c => c.Patent == patent.ToUpper() && c.Garage)
                    .Select(c => c.AdmissionDate)
                    .FirstOrDefault();

                return dischargeDate > admissionDate;
            }

            private bool ValidatePatent(string patent)
            {
                return _context.Cars.Any(p => string.Equals(p.Patent, patent.ToUpper()));
            }

        }

        public class ExitCarHandler : IRequestHandler<ExitCarCommand, CarResponse>
        {

            private readonly DbEstacionamientoContext _context;
            private readonly ExiteCarValidator _validator;
            private readonly IMapper _mapper;

            public ExitCarHandler(DbEstacionamientoContext context, ExiteCarValidator validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<CarResponse> Handle(ExitCarCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = _validator.Validate(request);
                    if (!validator.IsValid)
                    {
                        throw new ValidationException(validator.Errors);
                    }

                    var car = await _context.Cars
                    .Include(c => c.TypeNavigation)
                    .Include(c => c.BrandNavigation)
                    .Include(c => c.FormatNavigation)
                    .Where(c => c.Patent == request.Patent.ToUpper() && c.Garage).FirstOrDefaultAsync();

                    if (car == null)
                    {
                        throw new Exception("No existe el auto en el garage");
                    }

                    if (!car.Garage)
                    {
                        throw new Exception("El auto no se encuentra en el garage");
                    }
                    car.Garage = false;
                    car.Location = "None";
                    car.DischargeDate = request.DischargeDate;
                    car.Format = request.Format;
                    TimeSpan duration = request.DischargeDate - car.AdmissionDate;
                    Price? price = await _context.Prices.FindAsync(car.Type);
                    car.Amount = (decimal)duration.TotalHours * price.PriceName;

                    await _context.SaveChangesAsync();
                    var response = _mapper.Map<CarResponse>(car);
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}