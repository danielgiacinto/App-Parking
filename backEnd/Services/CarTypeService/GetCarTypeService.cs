using System.Reflection;
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;

namespace backEnd.Services.CarTypeService
{
    public class GetCarTypeService
    {

        public class GetCarTypeIdQuery : IRequest<CarTypeResponse>
        {
            public int IdCarType { get; set; }
        }

        public class GetCarTypeIdValidation : AbstractValidator<GetCarTypeIdQuery>
        {
            public GetCarTypeIdValidation()
            {
                RuleFor(p => p.IdCarType).NotEmpty().NotNull().NotEqual(0);
            }
        }

        public class GetAllBrandHandler : IRequestHandler<GetCarTypeIdQuery, CarTypeResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly GetCarTypeIdValidation _validator;

            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, GetCarTypeIdValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<CarTypeResponse> Handle(GetCarTypeIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = _validator.Validate(request);

                    if (!validator.IsValid)
                    {
                        throw new ValidationException(validator.Errors);
                    }
                    else
                    {
                        var carType = _context.CarTypes.Find(request.IdCarType);
                        if (carType == null)
                        {
                            throw new Exception("El tipo de auto no existe");
                        }
                        else
                        {
                            CarTypeResponse? response = _mapper.Map<CarTypeResponse>(carType);
                            return response;
                        }
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