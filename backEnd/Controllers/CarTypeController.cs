using backEnd.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.CarTypeService.GetCarTypeService;

namespace backEnd.Controllers {
    [ApiController]
    [Route("/carType")]
    public class CarTypeController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CarTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public Task<CarTypeResponse> GetCarType(int id){
            return _mediator.Send(new GetCarTypeIdQuery{IdCarType = id});
        }

    }
}