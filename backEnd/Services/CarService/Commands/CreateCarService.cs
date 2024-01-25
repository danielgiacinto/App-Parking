
using System.Data;
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarService.Commands
{

    public class CreateCarService
    {

        public class CreateCarCommand : IRequest<CarResponse>
        {
            public string Patent { get; set; } = null!;

            public int? Type { get; set; }

            public int? Brand { get; set; }

            public string? Model { get; set; }

            public DateTime AdmissionDate { get; set; }

            public int? State { get; set; }

            public string? Location { get; set; }
        }

        public class CreateCarValidator : AbstractValidator<CreateCarCommand>
        {

            private readonly DbEstacionamientoContext _context;
            public CreateCarValidator(DbEstacionamientoContext context)
            {
                _context = context;

                RuleFor(c => c.Patent).MinimumLength(6).MaximumLength(7).NotNull()
                .Must(ValidatePatent).WithMessage("El auto ya existe en el garage.");
                RuleFor(c => c.Type).NotEmpty().NotNull().NotEqual(0);
                RuleFor(c => c.Brand).NotEmpty().NotNull().NotEqual(0);
                RuleFor(c => c.Model).NotEmpty().NotNull().MaximumLength(25);
                RuleFor(c => c.AdmissionDate).NotNull()
                .Must(FechaActual).WithMessage("La fecha debe ser a la actual");
                RuleFor(c => c.State).NotEmpty().NotNull().NotEqual(0);
                RuleFor(c => c.Location).NotEmpty().NotNull().MaximumLength(4)
                .Must(ValidateLocation).WithMessage("La ubicacion ya esta siendo ocupada.");
            }

            private bool FechaActual(DateTime time)
            {
                if(time.Date == DateTime.Today){
                    return true;
                }
                return false;

            }

            private bool ValidateLocation(string location)
            {
                return !_context.Cars.Any(l => string.Equals(l.Location, location));
            }

            private bool ValidatePatent(string patent)
            {
                var existingGarageCar = _context.Cars.FirstOrDefault(c => string.Equals(c.Patent, patent.ToUpper()) && c.Garage);
                return existingGarageCar == null;
            }
        }

        public class CreateCarHandler : IRequestHandler<CreateCarCommand, CarResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly CreateCarValidator _validator;
            private readonly IMapper _mapper;

            public CreateCarHandler(DbEstacionamientoContext context, CreateCarValidator validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<CarResponse> Handle(CreateCarCommand request, CancellationToken cancellationToken)
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
                        Car car = _mapper.Map<Car>(request);
                        Guid IdCar = Guid.NewGuid();
                        car.IdCar = IdCar;
                        car.Patent = request.Patent.ToUpper();
                        car.Garage = true;
                        car.Amount = 0;
                        car.Format = 1;

                        await _context.AddAsync(car);
                        await _context.SaveChangesAsync();
                        car = _context.Cars.Include(c => c.TypeNavigation).Include(c => c.BrandNavigation).Include(c => c.StateNavigation).Include(c => c.FormatNavigation).First(c => c.IdCar == IdCar);
                        var responseCar = _mapper.Map<CarResponse>(car);
                        return responseCar;
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