using backEnd.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.StateService.GetStatusById;

namespace backEnd.Controllers {
    [ApiController]
    [Route("/status")]
    public class StatusController : ControllerBase
    {

        private readonly IMediator _mediator;

        public StatusController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public Task<StatusResponse> GetStatusById(int id){
            return _mediator.Send(new GetStatusByIdQuery {IdStatus = id});
        }

    }
}