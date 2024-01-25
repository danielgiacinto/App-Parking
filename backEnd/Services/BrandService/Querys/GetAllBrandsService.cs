
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.BrandService.Commands
{

    public class GetAllBrandsService
    {

        public class GetAllBrandQuery : IRequest<List<BrandResponse>>
        {
            public int IdBrand { get; set; }
            public string BrandName { get; set; } = null!;
        }

        public class GetAllBrandValidation : AbstractValidator<GetAllBrandQuery>
        {

            public GetAllBrandValidation()
            {
                RuleFor(p => p.IdBrand).NotEmpty();
                RuleFor(p => p.BrandName).NotEmpty();
            }
        }

        public class GetAllBrandHandler : IRequestHandler<GetAllBrandQuery, List<BrandResponse>>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly GetAllBrandValidation _validator;
            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, GetAllBrandValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<List<BrandResponse>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
            {
                _validator.Validate(request);
                try
                {
                    List<Brand> brands = await _context.Brands.ToListAsync();
                    List<BrandResponse> response = new();
                    if (brands.Count > 0)
                    {
                        foreach (Brand brand in brands)
                        {
                            BrandResponse brandResponseDto = new();
                            brandResponseDto = _mapper.Map<BrandResponse>(brand);
                            response.Add(brandResponseDto);
                        }
                    }

                    return response;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                throw new NotImplementedException();
            }
        }
    }
}