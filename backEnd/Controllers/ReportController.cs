using backEnd.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.CarService.Querys.GetAvailable;

namespace backEnd.Controllers {
    [ApiController]
    [Route("/reports")]
    public class ReportController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("availables")]
        public Task<AvailableResponse> GetQuantities(){
            return _mediator.Send(new GetAvailableCarQuery());
        }

    }
}