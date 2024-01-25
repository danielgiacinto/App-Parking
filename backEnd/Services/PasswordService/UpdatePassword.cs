using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.PasswordService
{

    public class UpdatePassword
    {
        public class UpdatePasswordCommand : IRequest<bool>
        {
            public string ActualPassword { get; set; } = null!;
            public string NewPassword { get; set; } = null!;
        }

        public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordCommand>
        {
            private readonly DbEstacionamientoContext _context;

            public UpdatePasswordValidator(DbEstacionamientoContext context)
            {
                _context = context;

                RuleFor(u => u.ActualPassword)
                    .NotEmpty().WithMessage("La contraseña actual es obligatoria")
                    .Must(ValidatePassword).WithMessage("La contraseña actual es incorrecta.");

                RuleFor(u => u.NewPassword)
                    .NotEmpty().WithMessage("La nueva contraseña no puede estar vacía.")
                    .MinimumLength(6).WithMessage("La nueva contraseña debe tener al menos 6 caracteres.")
                    .MaximumLength(25).WithMessage("La nueva contraseña no puede tener más de 25 caracteres.")
                    .Must((command, newPassword) => !ValidatePassword(newPassword))
                    .WithMessage("La nueva contraseña no puede ser igual a la actual.");
            }

            private bool ValidatePassword(string password)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email.Equals("administrador@estacionamiento.com"));
                if (user != null && password.Equals(user.Password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public class UpdatePasswordHandler : IRequestHandler<UpdatePasswordCommand, bool>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly UpdatePasswordValidator _validator;

            public UpdatePasswordHandler(DbEstacionamientoContext context, UpdatePasswordValidator validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<bool> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var validator = _validator.Validate(request);
                if (!validator.IsValid)
                {
                    throw new ValidationException(validator.Errors);
                }
                else
                {
                    var user = await _context.Users.Where(u => u.Email.Equals("administrador@estacionamiento.com")).FirstOrDefaultAsync();
                    user.Password = request.NewPassword;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
        }
    }
}