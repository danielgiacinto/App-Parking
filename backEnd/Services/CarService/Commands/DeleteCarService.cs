using AutoMapper;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.CarService.Commands
{

    public class DeleteCarService
    {
        public class DeleteCarCommand : IRequest<string>
        {
            public string Patent { get; set; } = null!;
        }

        public class DeleteCarValidation : AbstractValidator<DeleteCarCommand>
        {
            private readonly DbEstacionamientoContext _context;
            public DeleteCarValidation(DbEstacionamientoContext context)
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

        public class DeleteCarHandler : IRequestHandler<DeleteCarCommand, string>
        {
            private readonly DbEstacionamientoContext _context;

            private readonly DeleteCarValidation _validator;

            public DeleteCarHandler(DbEstacionamientoContext context, DeleteCarValidation validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<string> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
            {
                var validator = await _validator.ValidateAsync(request);
                if (!validator.IsValid)
                {
                    throw new ValidationException(validator.Errors);
                }
                else
                {
                    Car? car = await _context.Cars.FirstOrDefaultAsync(c => c.Patent == request.Patent.ToUpper());
                    if (car != null)
                    {
                        _context.Remove(car);
                        await _context.SaveChangesAsync();
                        return car.Patent;
                    }
                    else
                    {
                        return "No se elimino el registro del auto.";
                    }
                }
            }
        }
    }
}