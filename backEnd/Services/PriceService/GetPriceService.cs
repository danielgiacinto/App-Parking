using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.PriceService
{

    public class GetPriceService
    {

        public class GetPriceQuery : IRequest<Price>
        {
            public int Id { get; set; }
        }

        public class PriceUpdateValidator : AbstractValidator<GetPriceQuery>
        {

            public PriceUpdateValidator()
            {
                RuleFor(p => p.Id).NotEmpty().NotNull();
            }
        }

        public class PriceUpdateHandler : IRequestHandler<GetPriceQuery, Price>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly PriceUpdateValidator _validator;

            public PriceUpdateHandler(DbEstacionamientoContext context, PriceUpdateValidator validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<Price> Handle(GetPriceQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var validator = await _validator.ValidateAsync(request);
                    if (!validator.IsValid)
                    {
                        throw new ValidationException(validator.Errors);
                    }
                    Price? price = await _context.Prices.FindAsync(request.Id);
                    if (price == null)
                    {
                        throw new Exception("No existen datos en la base de datos");
                    }
                    return price;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}