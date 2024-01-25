using backEnd.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.PaymentService.PaymentService;

namespace backEnd.Controllers {
    [ApiController]
    [Route("/payments")]
    public class PaymentController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<PaymentResponse>> GetAllPayments(){
            return _mediator.Send(new GetAllPaymentsQuery());
        }

    }
}