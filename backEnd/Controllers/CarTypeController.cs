using backEnd.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.CarTypeService.GetCarTypeService;

namespace backEnd.Controllers {
    [ApiController]
    [Route("/carTypes")]
    public class CarTypeController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CarTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<List<CarTypeResponse>> GetCarType(){
            return _mediator.Send(new GetCarTypeIdQuery());
        }

    }
}