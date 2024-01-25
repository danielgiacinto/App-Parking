using backEnd.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.PriceService.GetPriceService;
using static backEnd.Services.PriceService.PriceUpdateService;

namespace backEnd.Controllers
{

    [ApiController]
    [Route("/price")]
    public class PriceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<Price> UpdatePrice(int id, [FromBody] PriceUpdateCommand command){
            command.Id = id;
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<Price> GetPrice(int id) {
            return await _mediator.Send(new GetPriceQuery {Id = id});
        }
    }
}