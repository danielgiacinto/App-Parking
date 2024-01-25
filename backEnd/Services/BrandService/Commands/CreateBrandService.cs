
using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.BrandService.Commands
{

    public class CreateBrandService
    {

        public class CreateBrandCommand : IRequest<BrandResponse>
        {
            public string BrandName { get; set; } = null!;
        }

        public class CreateBrandValidation : AbstractValidator<CreateBrandCommand>
        {

            public CreateBrandValidation()
            {
                RuleFor(p => p.BrandName).NotEmpty();
                RuleFor(p => p.BrandName).MinimumLength(2);
                RuleFor(p => p.BrandName).MaximumLength(20);
            }
        }

        public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, BrandResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly CreateBrandValidation _validator;
            private readonly IMapper _mapper;

            public CreateBrandHandler(DbEstacionamientoContext context, CreateBrandValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }
            public async Task<BrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
            {
                _validator.Validate(request);

                try
                {
                    var existe = await _context.Brands.AnyAsync(b => b.BrandName.Equals(request.BrandName));
                    if (existe)
                    {
                        throw new Exception("Ya existe esa marca de autos");
                    }
                    else
                    {
                        var brand = _mapper.Map<Brand>(request);

                        await _context.Brands.AddAsync(brand);
                        await _context.SaveChangesAsync();
                        
                        var brandResponse = _mapper.Map<BrandResponse>(brand);
                        return brandResponse;
                    }
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