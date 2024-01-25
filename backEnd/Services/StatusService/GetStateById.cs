using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;

namespace backEnd.Services.StateService {
    public class GetStatusById
    {

        public class GetStatusByIdQuery : IRequest<StatusResponse>
        {
            public int IdStatus { get; set; }
        }

        public class GetStateByIdValidation : AbstractValidator<GetStatusByIdQuery>
        {
            public GetStateByIdValidation()
            {
                RuleFor(p => p.IdStatus).NotEmpty().NotNull().NotEqual(0);
            }
        }

        public class GetStateByIdHandler : IRequestHandler<GetStatusByIdQuery, StatusResponse>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly GetStateByIdValidation _validator;

            private readonly IMapper _mapper;

            public GetStateByIdHandler(DbEstacionamientoContext context, GetStateByIdValidation validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<StatusResponse> Handle(GetStatusByIdQuery request, CancellationToken cancellationToken)
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
                        var status = _context.PaymentStatuses.Find(request.IdStatus);
                        if (status == null)
                        {
                            throw new Exception("El estado de pago no existe");
                        }
                        else
                        {
                            StatusResponse? response = _mapper.Map<StatusResponse>(status);
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