using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.Querys
{

    public class GetCarByPatent
    {
        public class GetCarByPatentQuery : IRequest<List<CarResponse>>
        {
            public string Patent { get; set; } = null!;
        }

        public class GetCarByPatentValidator : AbstractValidator<GetCarByPatentQuery>
        {
            private readonly DbEstacionamientoContext _context;
            public GetCarByPatentValidator(DbEstacionamientoContext context)
            {
                _context = context;
                RuleFor(c => c.Patent).MinimumLength(6).MaximumLength(7).NotNull()
                .MustAsync(ValidatePatent).WithMessage("La patente no existe en la base de datos.");
            }

            private async Task<bool> ValidatePatent(string patent, CancellationToken token)
            {
                return await _context.Cars.AnyAsync(c => c.Patent == patent.ToUpper());
            }
        }

        public class GetAllBrandHandler : IRequestHandler<GetCarByPatentQuery, List<CarResponse>>
        {
            private readonly DbEstacionamientoContext _context;

            private readonly GetCarByPatentValidator _validator;
            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, GetCarByPatentValidator validator, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<List<CarResponse>> Handle(GetCarByPatentQuery request, CancellationToken cancellationToken)
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
                    .Include(c => c.StateNavigation)
                    .Include(c => c.FormatNavigation)
                    .Where(c => c.Garage == false)
                    .Where(c => c.Location == "None")
                    .Where(c => c.Patent == request.Patent.ToUpper()).ToListAsync();
                    var carResponse = _mapper.Map<List<CarResponse>>(car);
                    return carResponse;
                }

            }
        }
    }
}