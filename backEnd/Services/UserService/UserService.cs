using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.UserService
{
    public class LoginUser
    {

        public class LoginUserCommand : IRequest<bool>
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginUserValidator : AbstractValidator<LoginUserCommand>
        {
            public LoginUserValidator()
            {
                RuleFor(u => u.Email).EmailAddress().NotEmpty();
                RuleFor(u => u.Password).NotEmpty();
            }
        }

        public class LoginUserHandler : IRequestHandler<LoginUserCommand, bool>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly LoginUserValidator _validator;

            public LoginUserHandler(DbEstacionamientoContext context, LoginUserValidator validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                _validator.Validate(request);
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == request.Password);
                    if (user != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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