using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.PriceService
{

    public class PriceUpdateService
    {

        public class PriceUpdateCommand : IRequest<Price>
        {
            public int Id { get; set; }

            public decimal PriceName { get; set; }
        }

        public class PriceUpdateValidator : AbstractValidator<PriceUpdateCommand>
        {

            public PriceUpdateValidator()
            {
                RuleFor(p => p.Id).NotEmpty().NotNull();
                RuleFor(p => p.PriceName).NotEmpty().NotNull();
            }
        }

        public class PriceUpdateHandler : IRequestHandler<PriceUpdateCommand, Price>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly PriceUpdateValidator _validator;

            public PriceUpdateHandler(DbEstacionamientoContext context, PriceUpdateValidator validator)
            {
                _context = context;
                _validator = validator;

            }
            public async Task<Price> Handle(PriceUpdateCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = await _validator.ValidateAsync(request);
                    if (!validator.IsValid)
                    {
                        throw new ValidationException(validator.Errors);
                    }
                    Price? priceUpdate = await _context.Prices.FirstOrDefaultAsync(p => p.Id.Equals(request.Id));
                    if (priceUpdate == null)
                    {
                        throw new Exception("No existe el id en la base de datos");
                    }
                    priceUpdate.PriceName = request.PriceName;
                    await _context.SaveChangesAsync();
                    return priceUpdate;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}