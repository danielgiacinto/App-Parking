using AutoMapper;
using backEnd.Models;
using FluentValidation;
using MediatR;
using static backEnd.Services.BrandService.Commands.CreateBrandService;

namespace backEnd.Services.BrandService.Commands
{
    public class DeleteBrandService
    {

        public class DeleteBrandCommand : IRequest<int>
        {
            public int IdBrand { get; set; }

        }

        public class DeleteBrandValidation : AbstractValidator<DeleteBrandCommand>
        {

            public DeleteBrandValidation()
            {
                RuleFor(b => b.IdBrand).NotEmpty();
            }
        }

        public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, int>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly DeleteBrandValidation _validator;
            private readonly IMapper _mapper;

            public DeleteBrandHandler(DbEstacionamientoContext context, DeleteBrandValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<int> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
            {
                var validator = _validator.Validate(request);
                if (validator.IsValid)
                {

                    try
                    {
                        Brand? existe = await _context.Brands.FindAsync(request.IdBrand);

                        if (existe != null)
                        {
                            _context.Remove(existe);
                            _context.SaveChanges();

                            return existe.IdBrand;
                        }
                        else
                        {
                            return 0;
                        }


                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                throw new NotImplementedException();
            }
        }
    }
}