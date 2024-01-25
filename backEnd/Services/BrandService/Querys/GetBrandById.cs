using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;

namespace backEnd.Services.BrandService.Querys
{

    public class GetBrandById
    {

        public class GetBrandIdQuery : IRequest<BrandResponse>
        {
            public int IdBrand { get; set; }
        }

        public class GetBrandIdValidation : AbstractValidator<GetBrandIdQuery>
        {
            public GetBrandIdValidation()
            {
                RuleFor(p => p.IdBrand).NotEmpty().NotNull().NotEqual(0);
            }
        }

        public class GetAllBrandHandler : IRequestHandler<GetBrandIdQuery, BrandResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly GetBrandIdValidation _validator;
            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, GetBrandIdValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<BrandResponse> Handle(GetBrandIdQuery request, CancellationToken cancellationToken)
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
                        var brand = _context.Brands.Find(request.IdBrand);
                        if(brand == null){
                            throw new Exception("La marca no existe");
                        }else {
                            BrandResponse? response = _mapper.Map<BrandResponse>(brand);
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