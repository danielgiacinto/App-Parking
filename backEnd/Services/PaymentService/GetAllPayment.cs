using AutoMapper;
using backEnd.Dto;
using backEnd.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backEnd.Services.PaymentService
{

    public class PaymentService
    {
        public class GetAllPaymentsQuery : IRequest<List<PaymentResponse>>
        {

        }


        public class GetAllBrandHandler : IRequestHandler<GetAllPaymentsQuery, List<PaymentResponse>>
        {
            private readonly DbEstacionamientoContext _context;
            private readonly IMapper _mapper;

            public GetAllBrandHandler(DbEstacionamientoContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PaymentResponse>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var payments = await _context.PaymentFormats.ToListAsync();
                    List<PaymentResponse> response = _mapper.Map<List<PaymentResponse>>(payments);
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}