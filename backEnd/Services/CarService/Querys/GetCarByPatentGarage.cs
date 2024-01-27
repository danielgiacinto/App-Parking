using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.Querys
{

    public class GetCarByPatentGarage
    {
        public class GetCarByPatentGarageQuery : IRequest<CarResponse>
        {
            public string Patent { get; set; } = null!;
        }

        public class GetCarByPatentGarageValidator : AbstractValidator<GetCarByPatentGarageQuery>
        {
            private readonly DbEstacionamientoContext _context;
            public GetCarByPatentGarageValidator(DbEstacionamientoContext context)
            {
                _context = context;
                RuleFor(c => c.Patent).MinimumLength(6).MaximumLength(7).NotNull()
                .MustAsync(ValidatePatent).WithMessage("La patente no existe en la base de datos.");
            }

            private object RuleFor(Func<object, object> value)
            {
                throw new NotImplementedException();
            }

            private async Task<bool> ValidatePatent(string patent, CancellationToken token)
            {
                return await _context.Cars.AnyAsync(c => c.Patent == patent.ToUpper());
            }
        }

        public class GetCarByPatentGarageHandler : IRequestHandler<GetCarByPatentGarageQuery, CarResponse>
        {
            private readonly DbEstacionamientoContext _context;

            private readonly GetCarByPatentGarageValidator _validator;
            private readonly IMapper _mapper;

            public GetCarByPatentGarageHandler(DbEstacionamientoContext context, GetCarByPatentGarageValidator validator, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<CarResponse> Handle(GetCarByPatentGarageQuery request, CancellationToken cancellationToken)
            {
                var validator = await _validator.ValidateAsync(request);
                if (!validator.IsValid)
                {
                    throw new ValidationException(validator.Errors);
                }
                else
                {
                    var car = await _context.Cars
                        .Include(c => c.TypeNavigation)
                        .Include(c => c.BrandNavigation)
                        .Include(c => c.FormatNavigation)
                        .Where(c => c.Patent == request.Patent.ToUpper())
                        .Where(c => c.Garage == true)
                        .Where(c => c.Location != "None")
                        .FirstOrDefaultAsync();
                    if (car != null)
                    {
                        var carResponse = _mapper.Map<CarResponse>(car);
                        return carResponse;
                    }
                    else
                    {
                        throw new Exception("La patente no existe en el garage o en la base de datos.");
                    }

                }
            }
        }
    }
}