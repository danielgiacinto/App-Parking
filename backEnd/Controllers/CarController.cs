using backEnd.Dto;
using backEnd.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static backEnd.Services.CarService.Commands.CreateCarService;
using static backEnd.Services.CarService.Commands.DeleteCarService;
using static backEnd.Services.CarService.Commands.ExitCarService;
using static backEnd.Services.CarService.Commands.UpdateCarService;
using static backEnd.Services.Querys.GetAllCarService;
using static backEnd.Services.Querys.GetAllCarsGarage;
using static backEnd.Services.Querys.GetCarByPatent;
using static backEnd.Services.Querys.GetCarByPatentGarage;

namespace backEnd.Controllers
{

    [ApiController]
    [Route("/cars")]
    public class CarController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public Task<CarResponse> CreateCar([FromBody] CreateCarCommand request)
        {
            return _mediator.Send(request);
        }

        [HttpGet]
        public Task<List<CarResponse>> GetAllCars()
        {
            return _mediator.Send(new GetAllCarQuery());
        }

        [HttpGet("{patent}")]
        public Task<List<CarResponse>> GetCarByPatent(string patent)
        {
            return _mediator.Send(new GetCarByPatentQuery { Patent = patent });
        }

        [HttpGet("/cars/garage")]
        public Task<List<CarResponse>> GetAllCarsGarage()
        {
            return _mediator.Send(new GetAllCarsGarageQuery());
        }

        [HttpGet("/cars/garage/{patent}")]
        public Task<CarResponse> GetCarByPatentGarage(string patent) {
            return _mediator.Send(new GetCarByPatentGarageQuery { Patent = patent });
        }

        [HttpPut("{patent}")]
        public Task<CarResponse> UpdateCar(string patent, [FromBody] UpdateCarCommand command)
        {
            command.Patent = patent;
            return _mediator.Send(command);
        }
        [HttpPut("/exit")]
        public Task<CarResponse> ExitCar([FromBody] ExitCarCommand command)
        {
            return _mediator.Send(command);
        }

        [HttpDelete("{patent}")]
        public Task<string> DeleteCar(string patent){
            return _mediator.Send(new DeleteCarCommand{Patent = patent});
        }
    }
}