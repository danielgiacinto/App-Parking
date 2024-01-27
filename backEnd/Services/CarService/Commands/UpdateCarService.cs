using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarService.Commands {

    public class UpdateCarService {
        public class UpdateCarCommand : IRequest<CarResponse>
        {
            public string Patent { get; set; } = null!;

            public int? Type { get; set; }

            public int? Brand { get; set; }

            public DateTime AdmissionDate { get; set; }

            public DateTime DischargeDate { get; set; }

            public string? Location { get; set; }
        }

        public class UpdateCarValidator : AbstractValidator<UpdateCarCommand>
        {

            private readonly DbEstacionamientoContext _context;
            public UpdateCarValidator(DbEstacionamientoContext context)
            {
                _context = context;

                RuleFor(c => c.Patent).MinimumLength(6).MaximumLength(7).NotNull()
                .Must(ValidatePatent).WithMessage("La patente no existe en la base de datos.");
                RuleFor(c => c.Type).NotEmpty().NotNull().NotEqual(0);
                RuleFor(c => c.Brand).NotEmpty().NotNull().NotEqual(0);
                RuleFor(c => c.AdmissionDate).NotNull();
                RuleFor(c => c.DischargeDate).NotNull()
                .Must((before, after) => ValidateDate(before.AdmissionDate, after))
                .WithMessage("La fecha no es correcta, la fecha de ingreso no puede ser posterior a la de egreso.");
                RuleFor(c => c.Location).NotEmpty().NotNull().MaximumLength(4)
                .Must(ValidateLocation).WithMessage("La ubicacion ya esta siendo ocupada.");
            }

            private bool ValidateLocation(string location)
            {
                return !_context.Cars.Any(l => string.Equals(l.Location, location));
            }

            private bool ValidatePatent(string patent)
            {
                return _context.Cars.Any(p => string.Equals(p.Patent, patent.ToUpper()));
            }

            private bool ValidateDate(DateTime admissionDate, DateTime after)
            {
                if (after >= admissionDate && admissionDate <= after && admissionDate != after)
                    return true;
                return false;
            }
        }

        public class UpdateCarHandler : IRequestHandler<UpdateCarCommand, CarResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly UpdateCarValidator _validator;
            private readonly IMapper _mapper;

            public UpdateCarHandler(DbEstacionamientoContext context, UpdateCarValidator validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<CarResponse> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
            {

                try
                {
                    var validator = await _validator.ValidateAsync(request);
                    if (!validator.IsValid)
                    {
                        throw new ValidationException(validator.Errors);
                    }
                    else
                    {
                        var carExist = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == request.Patent.ToUpper() && c.Garage == true);
                        if (carExist == null)
                        {
                            throw new Exception("El auto no existe en el garage.");
                        }
                        carExist.Type = request.Type;
                        carExist.Brand = request.Brand;
                        carExist.AdmissionDate = request.AdmissionDate;
                        carExist.DischargeDate = request.DischargeDate;
                        TimeSpan duration = request.DischargeDate - request.AdmissionDate;
                        Price? price = await _context.Prices.FindAsync(1);
                        carExist.Amount = (decimal)duration.TotalHours * price.PriceName;
                        carExist.Location = request.Location;

                        await _context.SaveChangesAsync();
                        var response = _mapper.Map<CarResponse>(carExist);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
        }
    }
}